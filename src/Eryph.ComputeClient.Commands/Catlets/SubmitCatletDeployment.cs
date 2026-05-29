using System;
using System.Collections;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Text.Json;
using Azure;
using Eryph.ComputeClient.Models;
using Eryph.ConfigModel.Json;
using JetBrains.Annotations;
using Operation = Eryph.ComputeClient.Models.Operation;

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

        Operation operation;
        try
        {
            operation = Factory.CreateCatletSpecificationsClient().Deploy(
                specificationId,
                specificationVersionId,
                new DeployCatletSpecificationRequestBody(variables.ToDictionary(v => v.Name, v => v.Value))
                {
                    Architecture = Architecture,
                    Redeploy = Redeploy,
                });
        }
        catch (RequestFailedException ex) when (ex.Status == (int)HttpStatusCode.Conflict && !Redeploy)
        {
            // The specification is already deployed as a catlet. Surface the server's
            // detail (it names the existing catlet) and point at the actionable fix
            // instead of leaking the raw HTTP response.
            var detail = TryGetProblemDetail(ex) ?? "The catlet specification is already deployed.";
            WriteError(new ErrorRecord(
                new InvalidOperationException(
                    $"{detail} Re-run with -Redeploy to replace the existing catlet (the existing catlet will be deleted)."),
                "CatletSpecificationAlreadyDeployed",
                ErrorCategory.ResourceExists,
                specificationId));
            return;
        }

        WaitForCatlet(operation, NoWait);
    }

    // Extracts the 'detail' field of an RFC 9457 problem+json response, if present.
    private static string TryGetProblemDetail(RequestFailedException ex)
    {
        try
        {
            var content = ex.GetRawResponse()?.Content;
            if (content is null)
                return null;

            using var document = JsonDocument.Parse(content.ToString());
            return document.RootElement.TryGetProperty("detail", out var detail)
                ? detail.GetString()
                : null;
        }
        catch
        {
            return null;
        }
    }

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
