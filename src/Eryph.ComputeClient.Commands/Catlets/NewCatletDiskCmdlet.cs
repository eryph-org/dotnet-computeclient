using Eryph.ComputeClient.Models;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using Eryph.ComputeClient.Commands.ProjectMembers;

namespace Eryph.ComputeClient.Commands.Catlets;

[PublicAPI]
[Cmdlet(VerbsCommon.New, "CatletDisk")]
[OutputType(typeof(Operation), typeof(VirtualDisk))]
public class NewCatletDiskCmdlet : CatletDiskCmdlet
{
    [Parameter(Mandatory = true)]
    [ValidateNotNullOrEmpty]
    public string Name { get; set; }

    [Parameter(Mandatory = true)]
    [ValidateRange(0, int.MaxValue)]
    public int Size { get; set; }

    [Parameter(Mandatory = true)]
    [ValidateNotNullOrEmpty]
    public string ProjectName { get; set; }

    [Parameter]
    [ValidateNotNullOrEmpty]
    public string Store { get; set; }

    [Parameter]
    [ValidateNotNullOrEmpty]
    public string Environment { get; set; }

    [Parameter]
    public SwitchParameter NoWait { get; set; }

    protected override void ProcessRecord()
    {
        var projectId = GetProjectId(ProjectName);


        var recordId = Guid.NewGuid();
        WaitForOperation(
            Factory.CreateVirtualDisksClient().Create(
                new NewVirtualDiskRequest(projectId.GetValueOrDefault(), Name, Size)
                {
                    CorrelationId = recordId,
                    Environment = Environment,
                    Store = Store,
                    Location = "blub",
                    // TODO add location/storage identifier
                }),
            NoWait,
            true);
    }
}