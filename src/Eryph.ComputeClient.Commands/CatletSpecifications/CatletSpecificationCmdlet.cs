using Eryph.ComputeClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.CatletSpecifications;

[PublicAPI]
public abstract class CatletSpecificationCmdlet : ComputeCmdLet
{
    protected CatletSpecification GetSingleCatletSpecification(string id)
    {
        return Factory.CreateCatletSpecificationsClient().Get(id);
    }

    protected void WaitForOperation(
        Operation operation,
        bool noWait,
        bool alwaysWriteMachine,
        string knownMachineId = null)
    {
        if (noWait)
        {
            if (knownMachineId == null || !alwaysWriteMachine)
                WriteObject(operation);
            else
                WriteObject(GetSingleCatletSpecification(knownMachineId));
            return;
        }

        var completedOperation = WaitForOperation(operation);
        WriteResources(completedOperation, ResourceType.Catlet);
    }
}
