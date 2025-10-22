using System.Collections;
using System.Linq;
using System.Management.Automation;
using Eryph.ComputeClient.Models;
using Eryph.ConfigModel.Json;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.Catlets;

[PublicAPI]
[Cmdlet("Deploy", "Catlet")]
[OutputType(typeof(Operation))]
public class DeployCatlet : CatletConfigCmdlet
{
    [Parameter(
        ParameterSetName = "InputObject",
        Position = 0,
        ValueFromPipeline = true,
        Mandatory = true)]
    [AllowEmptyString]
    public string SpecificationId { get; set; }

    [Parameter]
    public Hashtable Variables { get; set; }

    [Parameter]
    public SwitchParameter SkipVariablesPrompt { get; set; }

    [Parameter]
    public SwitchParameter NoWait { get; set; }

    protected override void ProcessRecord()
    {
        if (string.IsNullOrEmpty(SpecificationId))
            return;

        var specificationsClient = Factory.CreateCatletSpecificationsClient();
        var specification = specificationsClient.Get(SpecificationId);

        var latestVersion = Factory.CreateCatletSpecificationsClient().GetVersion(
            specification.Value.Id,
            specification.Value.Latest.Id);

        var resolvedConfig = CatletConfigJsonSerializer.Deserialize(latestVersion.Value.ResolvedConfig);
        var variables = resolvedConfig.Variables ?? [];

        if (!PopulateVariables(variables, Variables, SkipVariablesPrompt, false))
            return;

        WaitForCatlet(
            Factory.CreateCatletSpecificationsClient().Deploy(
                SpecificationId,
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
