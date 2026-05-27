using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.Catlets;

[PublicAPI]
[Cmdlet(VerbsCommon.Remove, "CatletDisk")]
[OutputType(typeof(Operation), typeof(VirtualDisk))]
public class RemoveCatletDiskCmdlet : CatletDiskCmdlet
{
    [Parameter(
        Position = 0,
        ValueFromPipeline = true,
        Mandatory = true,
        ValueFromPipelineByPropertyName = true)]
    [Alias("Name")]
    public string[] Id { get; set; }

    /// <summary>
    /// This parameter overrides the ShouldContinue call to force
    /// the cmdlet to stop its operation. This parameter should always
    /// be used with caution.
    /// </summary>
    [Parameter]
    public SwitchParameter Force { get; set; }

    /// <summary>
    /// This parameter indicates that the cmdlet should return
    /// an object to the pipeline after the processing has been
    /// completed.
    /// </summary>
    [Parameter]
    public SwitchParameter PassThru { get; set; }

    [Parameter]
    public SwitchParameter NoWait { get; set; }

    [Parameter]
    [ValidateNotNullOrEmpty]
    public string ProjectName { get; set; }

    private bool _yesToAll;
    private bool _noToAll;

    protected override void ProcessRecord()
    {
        foreach (var nameOrId in Id)
        {
            foreach (var virtualDisk in ResolveActionTargets(nameOrId, ProjectName, GetSingleCatletDisk,
                         projectId => Factory.CreateVirtualDisksClient().List(projectId: projectId),
                         d => d.Name, "catlet disk"))
            {
                if (Stopping) break;

                if (!Force && !ShouldContinue($"Catlet disk '{virtualDisk.Name}' (Id:{virtualDisk.Id}) will be deleted!", "Warning!",
                        ref _yesToAll, ref _noToAll))
                {
                    continue;
                }

                WaitForOperation(Factory.CreateVirtualDisksClient().Delete(virtualDisk.Id).Value, NoWait, false, virtualDisk.Id);

                if (PassThru)
                    WriteObject(virtualDisk);
            }
        }
    }
}
