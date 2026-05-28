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

    [Parameter(ParameterSetName = "list")]
    [ValidateNotNullOrEmpty]
    public string Location { get; set; }

    [Parameter(ParameterSetName = "list")]
    [ValidateNotNullOrEmpty]
    public string DataStore { get; set; }

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

        if (!TryGetProjectId(ProjectName, out var projectId))
            return;

        // A disk is identified by Name + Location + DataStore (within a project), so
        // -Name alone may not be unique. Allow narrowing the listing by -Environment,
        // -Location and -DataStore (case-insensitive exact match). An explicit id
        // (GUID) bypasses these filters.
        IEnumerable<VirtualDisk> disks = Factory.CreateVirtualDisksClient().List(projectId: projectId);
        if (!string.IsNullOrWhiteSpace(Environment))
            disks = disks.Where(d => string.Equals(d.Environment, Environment, StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrWhiteSpace(Location))
            disks = disks.Where(d => string.Equals(d.Location, Location, StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrWhiteSpace(DataStore))
            disks = disks.Where(d => string.Equals(d.DataStore, DataStore, StringComparison.OrdinalIgnoreCase));

        WriteByNameOrId(Name, GetSingleCatletDisk, () => disks, d => d.Name, "catlet disk");
    }
}
