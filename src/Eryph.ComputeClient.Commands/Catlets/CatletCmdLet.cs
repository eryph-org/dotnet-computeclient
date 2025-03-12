using Eryph.ComputeClient.Models;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.Catlets
{
    [PublicAPI]
    public abstract class CatletCmdLet : ComputeCmdLet
    {
        protected Catlet GetSingleCatlet(string id)
        {
            return Factory.CreateCatletsClient().Get(id);
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
                    WriteObject(GetSingleCatlet(knownMachineId));
                return;
            }

            var completedOperation = WaitForOperation(operation);
            WriteResources(completedOperation, ResourceType.Catlet);
        }
    }
}