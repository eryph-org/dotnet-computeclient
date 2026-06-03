using Eryph.ComputeClient.Models;
using Eryph.ConfigModel.Json;
using Eryph.ConfigModel.Yaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.CatletSpecifications;

[PublicAPI]
[Cmdlet(VerbsCommon.Get, "CatletSpecification", DefaultParameterSetName = "list")]
[OutputType(typeof(CatletSpecification), ParameterSetName = ["get"])]
[OutputType(typeof(CatletSpecification), ParameterSetName = ["list"])]
[OutputType(typeof(string), ParameterSetName = ["get"])]
[OutputType(typeof(string), ParameterSetName = ["list"])]
public class GetCatletSpecification : CatletSpecificationCmdlet
{
    [Parameter(
        ParameterSetName = "get",
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true)]
    public string[] Id { get; set; }

    [Parameter(
        ParameterSetName = "list",
        ValueFromPipelineByPropertyName = true)]
    [ValidateNotNullOrEmpty]
    public string ProjectName { get; set; }

    [Parameter(
        ParameterSetName = "list",
        Position = 0)]
    [ValidateNotNullOrEmpty]
    public string Name { get; set; }

    // Return the configuration content of each specification's latest version as a
    // string instead of the specification object (mirrors 'Get-CatletSpecificationVersion -Config').
    [Parameter]
    public SwitchParameter Config { get; set; }

    protected override void ProcessRecord()
    {
        if (Id is not null)
        {
            foreach (var id in Id)
            {
                if (Stopping) break;
                if (TryGetById(id, GetSingleCatletSpecification, "catlet specification", out var specification))
                {
                    if (Config.IsPresent)
                        WriteSpecificationConfig(specification);
                    else
                        WriteObject(specification);
                }
            }

            return;
        }

        if (!TryGetProjectId(ProjectName, out var projectId))
            return;

        Action<CatletSpecification> writeItem = Config.IsPresent ? WriteSpecificationConfig : null;
        WriteByNameOrId(
            Name,
            GetSingleCatletSpecification,
            () => Factory.CreateCatletSpecificationsClient().List(projectId: projectId),
            specification => specification.Name,
            "catlet specification",
            writeItem);
    }

    // Writes the configuration content of the specification's latest version.
    private void WriteSpecificationConfig(CatletSpecification specification)
    {
        if (specification.Latest is null)
        {
            WriteError(new ErrorRecord(
                new ItemNotFoundException(
                    $"The catlet specification '{specification.Name}' does not have any versions."),
                "NoVersions",
                ErrorCategory.ObjectNotFound,
                specification));
            return;
        }

        var version = Factory.CreateCatletSpecificationsClient()
            .GetVersion(specification.Id, specification.Latest.Id).Value;
        WriteObject(version.Configuration.Content);
    }
}
