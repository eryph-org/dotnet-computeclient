using System.Management.Automation;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.CatletSpecifications;

[PublicAPI]
[Cmdlet(VerbsCommon.Get, "CatletSpecificationVersion", DefaultParameterSetName = "get")]
[OutputType(typeof(CatletSpecificationVersion), ParameterSetName = ["get"])]
[OutputType(typeof(CatletSpecificationVersionInfo), ParameterSetName = ["list"])]
public class GetCatletSpecificationVersion : CatletSpecificationCmdlet
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
    [Parameter(ParameterSetName = "get", Mandatory = true)]
    [ValidateNotNullOrEmpty]
    public string SpecificationId { get; set; }

    protected override void ProcessRecord()
    {
        if (Id is not null)
        {
            var specificationsClient = Factory.CreateCatletSpecificationsClient();
            foreach (var id in Id)
            {
                WriteObject(specificationsClient.GetVersion(SpecificationId, id).Value);
            }

            return;
        }

        var specificationVersions = Factory.CreateCatletSpecificationsClient()
            .ListVersions(specificationId: SpecificationId);
        foreach (var specification in specificationVersions)
        {
            if (Stopping) break;

            WriteObject(specification, true);
        }
    }
}
