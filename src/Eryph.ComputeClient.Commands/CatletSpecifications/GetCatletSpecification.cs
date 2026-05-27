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
[Cmdlet(VerbsCommon.Get, "CatletSpecification", DefaultParameterSetName = "get")]
[OutputType(typeof(CatletSpecification), ParameterSetName = ["get"])]
[OutputType(typeof(CatletSpecification), ParameterSetName = ["list"])]
public class GetCatletSpecification : CatletSpecificationCmdlet
{
    [Parameter(
        ParameterSetName = "get",
        Position = 0,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true)]
    public string[] Id { get; set; }

    [Parameter(
        ParameterSetName = "list",
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true)]
    [ValidateNotNullOrEmpty]
    public string ProjectName { get; set; }

    [Parameter(
        ParameterSetName = "list",
        ValueFromPipelineByPropertyName = true)]
    [ValidateNotNullOrEmpty]
    public string Name { get; set; }

    protected override void ProcessRecord()
    {
        if (Id is not null)
        {
            foreach (var id in Id)
            {
                WriteObject(GetSingleCatletSpecification(id));
            }

            return;
        }

        var projectId = GetProjectId(ProjectName);
        WriteFilteredByName(
            Factory.CreateCatletSpecificationsClient().List(projectId: projectId),
            Name,
            specification => specification.Name);
    }
}
