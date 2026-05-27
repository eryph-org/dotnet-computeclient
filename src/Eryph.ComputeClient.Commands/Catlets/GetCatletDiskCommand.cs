using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.Catlets;

[PublicAPI]
[Cmdlet(VerbsCommon.Get, "CatletDisk", DefaultParameterSetName = "list")]
[OutputType(typeof(VirtualDisk), ParameterSetName = ["get"])]
[OutputType(typeof(VirtualDisk), ParameterSetName = ["list"])]
public class GetCatletDiskCommand : CatletDiskCmdlet
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

    [Parameter(ParameterSetName = "list")]
    [ValidateNotNullOrEmpty]
    public string Environment { get; set; }

    protected override void ProcessRecord()
    {
        if (Id != null)
        {
            foreach (var id in Id)
            {
                if (Stopping) break;
                if (TryGetById(id, GetSingleCatletDisk, "catlet disk", out var disk))
                    WriteObject(disk);
            }

            return;
        }

        var projectId = GetProjectId(ProjectName);

        // Disk names are unique per project + environment, so allow narrowing by
        // environment. The environment filter is applied to the listing; an explicit
        // id (GUID) ignores it.
        IEnumerable<VirtualDisk> disks = Factory.CreateVirtualDisksClient().List(projectId: projectId);
        if (!string.IsNullOrWhiteSpace(Environment))
            disks = disks.Where(d => string.Equals(d.Environment, Environment, StringComparison.OrdinalIgnoreCase));

        WriteByNameOrId(Name, GetSingleCatletDisk, () => disks, d => d.Name, "catlet disk");
    }
}
