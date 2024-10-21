using System.Management.Automation;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.Catlets;

[PublicAPI]
[Cmdlet(VerbsCommon.Get, "CatletDisk", DefaultParameterSetName = "get")]
[OutputType(typeof(VirtualDisk), ParameterSetName = ["get"])]
[OutputType(typeof(VirtualDisk), ParameterSetName = ["list"])]
public class GetCatletDiskCommand : CatletDiskCmdlet
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

    protected override void ProcessRecord()
    {
        if (Id != null)
        {
            foreach (var id in Id)
            {
                WriteObject(GetSingleCatletDisk(id));
            }

            return;
        }

        var projectId = GetProjectId(ProjectName);
        ListOutput(Factory.CreateVirtualDisksClient().List(projectId: projectId));
    }
}
