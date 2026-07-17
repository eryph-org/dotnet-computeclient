using System;
using System.Collections;
using System.Linq;
using System.Management.Automation;
using Eryph.ComputeClient.Models;
using Eryph.ConfigModel.Json;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.Catlets;

[PublicAPI]
[Cmdlet("Submit", "CatletDeployment")]
[Alias("Deploy-Catlet")]
[OutputType(typeof(Operation))]
public class SubmitCatletDeployment : CatletConfigCmdlet
{
    [Parameter(
        ParameterSetName = "Parameter",
        Mandatory = true)]
    [ValidateNotNullOrEmpty]
    public string SpecificationId { get; set; }

    [Parameter(
        ParameterSetName = "Parameter",
        Mandatory = true)]
    [ValidateNotNullOrEmpty]
    public string SpecificationVersionId { get; set; }

    [Parameter(
        ParameterSetName = "Pipeline",
        Position = 0,
        Mandatory = true,
        ValueFromPipeline= true)]
    public CatletSpecificationVersion Version { get; set; }

    // Deploy a specification's latest version directly, e.g.
    // 'Get-CatletSpecification -Name foo | Deploy-Catlet'.
    [Parameter(
        ParameterSetName = "Specification",
        Position = 0,
        Mandatory = true,
        ValueFromPipeline = true)]
    public CatletSpecification Specification { get; set; }

    [Parameter]
    public string Architecture { get; set; }

    // A specification is project level and deploys into many environments, so the environment
    // belongs to the deployment rather than to the specification.
    [Parameter]
    public string Environment { get; set; }

    [Parameter]
    public Hashtable Variables { get; set; }

    [Parameter]
    public SwitchParameter Redeploy { get; set; }

    [Parameter]
    public SwitchParameter SkipVariablesPrompt { get; set; }

    [Parameter]
    public SwitchParameter Force { get; set; }

    [Parameter]
    public SwitchParameter NoWait { get; set; }

    protected override void BeginProcessing()
    {
        base.BeginProcessing();
        RequireApiVersion(1, 2, "Deploy-Catlet");
    }

    protected override void ProcessRecord()
    {
        if (Specification is not null && Specification.Latest is null)
        {
            WriteError(new ErrorRecord(
                new InvalidOperationException(
                    $"The catlet specification '{Specification.Name}' does not have any versions to deploy."),
                "NoVersions",
                ErrorCategory.ObjectNotFound,
                Specification));
            return;
        }

        var specificationId = Version?.SpecificationId ?? Specification?.Id ?? SpecificationId;
        var specificationVersionId = Version?.Id ?? Specification?.Latest?.Id ?? SpecificationVersionId;
        if (specificationId is null || specificationVersionId is null)
            return;

        // A specification has one deployment per environment it is deployed into, so only a
        // deployment in the environment we are targeting is a conflict — the same specification in
        // another environment is exactly what deploying per environment is for. The piped
        // specification already tells us; for other inputs we ask the server. Detecting this up
        // front lets us point at -Redeploy instead of failing the deployment with a backend conflict.
        if (!Redeploy)
        {
            var deployments = Specification is not null
                ? Specification.Deployments
                : Factory.CreateCatletSpecificationsClient().Get(specificationId).Value.Deployments;

            var deployedCatletId = deployments?
                .FirstOrDefault(d => string.Equals(
                    d.Environment, TargetEnvironment, StringComparison.OrdinalIgnoreCase))
                ?.CatletId;

            if (!string.IsNullOrEmpty(deployedCatletId))
            {
                WriteError(new ErrorRecord(
                    new InvalidOperationException(
                        $"The catlet specification is already deployed in the environment "
                        + $"'{TargetEnvironment}' as catlet {deployedCatletId}. Re-run with -Redeploy "
                        + "to replace the existing catlet (the existing catlet will be deleted), or "
                        + "deploy into another environment with -Environment."),
                    "CatletSpecificationAlreadyDeployed",
                    ErrorCategory.ResourceExists,
                    specificationId));
                return;
            }
        }

        var specificationVersion = Factory.CreateCatletSpecificationsClient().GetVersion(
            specificationId,
            specificationVersionId);

        CatletSpecificationVersionVariant variant;
        if (Architecture is not null)
        {
            variant = specificationVersion.Value.Variants.FirstOrDefault(v => v.Architecture == Architecture);
            if (variant is null)
            {
                WriteError(new ErrorRecord(
                    new InvalidOperationException($"The specification version does not support the architecture '{Architecture}'."),
                    "ArchitectureNotFound",
                    ErrorCategory.ObjectNotFound,
                    Architecture));
                return;
            }
        }
        else
        {
            if (specificationVersion.Value.Variants.Count > 1)
            {
                WriteError(new ErrorRecord(
                    new InvalidOperationException("The specification supports multiple architectures. Please specify the architecture."),
                    "ArchitectureNotSpecified",
                    ErrorCategory.InvalidOperation,
                    specificationVersion));
                return;
            }

            variant = specificationVersion.Value.Variants[0];
        }

        if (Redeploy && !Force && !ShouldContinue("Catlet specification will be redeployed. An existing catlet might be deleted!", "Warning!"))
        {
            return;
        }

        var resolvedConfig = CatletConfigJsonSerializer.Deserialize(variant.BuiltConfig);
        var variables = resolvedConfig.Variables ?? [];

        if (!PopulateVariables(variables, Variables, SkipVariablesPrompt, false))
            return;

        WaitForCatlet(
            Factory.CreateCatletSpecificationsClient().Deploy(
                specificationId,
                specificationVersionId,
                new DeployCatletSpecificationRequestBody(variables.ToDictionary(v => v.Name, v => v.Value))
                {
                    Architecture = Architecture,
                    Redeploy = Redeploy,
                    Environment = Environment,
                }),
            NoWait);
    }

    /// <summary>
    /// The environment this deployment targets. Left unset, the server deploys into the default
    /// environment, so the conflict check has to compare against the same name.
    /// </summary>
    private string TargetEnvironment =>
        string.IsNullOrWhiteSpace(Environment) ? "default" : Environment;

    private void WaitForCatlet(Operation operation, bool noWait)
    {
        if (noWait)
        {
            WriteObject(operation);
            return;
        }
        
        var completedOperation = WaitForOperation(operation);
        if (completedOperation.Result is not CatletOperationResult configResult)
            return;

        var catlet = GetSingleCatlet(configResult.CatletId);
        WriteObject(catlet);
    }
}
