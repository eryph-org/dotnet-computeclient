using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Eryph.ComputeClient.Models;
using Eryph.ConfigModel.Json;
using Eryph.ConfigModel.Variables;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.Catlets;

[PublicAPI]
[Cmdlet("Deploy", "Catlet")]
[OutputType(typeof(Operation))]
public class DeployCatlet : CatletConfigCmdlet
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

    // TODO does the pipeline work?
    [Parameter(
        ParameterSetName = "Pipeline",
        Position = 0,
        Mandatory = true,
        ValueFromPipeline= true)]
    public CatletSpecificationVersion Version { get; set; }

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

    private bool _yesToAll;
    private bool _noToAll;

    protected override void ProcessRecord()
    {
        var specificationId = Version?.SpecificationId ?? SpecificationId;
        var specificationVersionId = Version?.Id ?? SpecificationVersionId;
        if (specificationId is null || specificationVersionId is null)
            return;

        var specificationVersion = Factory.CreateCatletSpecificationsClient().GetVersion(
            specificationId,
            specificationVersionId);

        CatletSpecificationVersionVariant variant;
        if (Architecture is not null)
        {
            variant = specificationVersion.Value.Variants.FirstOrDefault(v => v.Architecture == Architecture);
            WriteError(new ErrorRecord(
                new InvalidOperationException($"The specification version does not support the architecture '{Architecture}'."),
                "ArchitectureNotFound",
                ErrorCategory.ObjectNotFound,
                Architecture));
            return;
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

        if (Redeploy && !Force && !ShouldContinue("Catlet specification will be redeployed. An existing catlet might be deleted!", "Warning!",
                ref _yesToAll, ref _noToAll))
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
                new DeployCatletSpecificationRequestBody(variables.ToDictionary(v => v.Name, v => v.Value))),
            NoWait);
    }

    private void WaitForCatlet(Operation operation, bool noWait)
    {
        if (noWait)
        {
            WriteObject(operation);
            return;
        }

        var completedOperation = WaitForOperation(operation);
        WriteResources(completedOperation, ResourceType.Catlet);
    }
}
