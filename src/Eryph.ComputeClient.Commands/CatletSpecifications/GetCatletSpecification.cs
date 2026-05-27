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

    protected override void ProcessRecord()
    {
        if (Id is not null)
        {
            foreach (var id in Id)
            {
                if (Stopping) break;
                if (TryGetById(id, GetSingleCatletSpecification, "catlet specification", out var specification))
                    WriteObject(specification);
            }

            return;
        }

        if (!TryGetProjectId(ProjectName, out var projectId))
            return;

        WriteByNameOrId(
            Name,
            GetSingleCatletSpecification,
            () => Factory.CreateCatletSpecificationsClient().List(projectId: projectId),
            specification => specification.Name,
            "catlet specification");
    }
}
