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
[Cmdlet(VerbsCommon.Remove, "CatletDisk", DefaultParameterSetName = "Wait")]
[OutputType(typeof(VirtualDisk), ParameterSetName = new[] { "Wait" })]
[OutputType(typeof(Operation), ParameterSetName = new[] { "NoWait" })]
public class RemoveCatletDiskCmdlet : CatletDiskCmdlet
{
    // A disk is identified by Name + Location + DataStore (within a project), so
    // -Name alone cannot uniquely identify the target of a destructive operation.
    // Remove-CatletDisk therefore accepts only ids; use Get-CatletDisk (which
    // supports -Name plus -Environment / -Location / -DataStore filters) to find
    // the disk first and pipe it in.
    [Parameter(
        Position = 0,
        ValueFromPipeline = true,
        Mandatory = true,
        ValueFromPipelineByPropertyName = true)]
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

    [Parameter(ParameterSetName = "NoWait")]
    public SwitchParameter NoWait { get; set; }

    private bool _yesToAll;
    private bool _noToAll;

    protected override void ProcessRecord()
    {
        foreach (var id in Id)
        {
            if (Stopping) break;

            if (!TryGetById(id, GetSingleCatletDisk, "catlet disk", out var virtualDisk))
                continue;

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
