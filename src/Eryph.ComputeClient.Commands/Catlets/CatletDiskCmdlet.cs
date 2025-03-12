using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eryph.ComputeClient.Models;

namespace Eryph.ComputeClient.Commands.Catlets;

public abstract class CatletDiskCmdlet : ComputeCmdLet
{
    protected VirtualDisk GetSingleCatletDisk(string id)
    {
        return Factory.CreateVirtualDisksClient().Get(id);
    }

    protected void WaitForOperation(
        Operation operation,
        bool noWait,
        bool alwaysWriteCatletDisk,
        string knownCatletDiskId = null)
    {
        if (noWait)
        {
            if (knownCatletDiskId == null || !alwaysWriteCatletDisk)
                WriteObject(operation);
            else
                WriteObject(GetSingleCatletDisk(knownCatletDiskId));
            return;
        }

        var completedOperation = WaitForOperation(operation);
        WriteResources(completedOperation, ResourceType.VirtualDisk);
    }
}
