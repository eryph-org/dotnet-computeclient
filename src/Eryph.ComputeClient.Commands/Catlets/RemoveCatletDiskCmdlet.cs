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

    private bool _yesToAll;
    private bool _noToAll;

    protected override void ProcessRecord()
    {
        foreach (var id in Id)
        {
            VirtualDisk virtualDisk;
            try
            {
                virtualDisk = Factory.CreateVirtualDisksClient().Get(id);
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "CatletDiskNotFound", ErrorCategory.ObjectNotFound, id));
                continue;
            }

            if (!Force && !ShouldContinue($"Catlet disk '{virtualDisk.Name}' (Id:{id}) will be deleted!", "Warning!",
                    ref _yesToAll, ref _noToAll))
            {
                continue;
            }

            WaitForOperation(Factory.CreateVirtualDisksClient().Delete(id).Value, NoWait.IsPresent, false, id);

            if (PassThru)
                WriteObject(virtualDisk);
        }
    }
}
