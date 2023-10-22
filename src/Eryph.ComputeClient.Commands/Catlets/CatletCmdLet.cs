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

        protected void WaitForOperation(Operation operation, bool noWait, bool alwaysWriteMachine, string knownMachineId = default)
        {
            if (noWait)
            {
                if (knownMachineId == default || !alwaysWriteMachine)
                    WriteObject(operation);
                else
                    WriteObject(GetSingleCatlet(knownMachineId));
                return;
            }

            WaitForOperation(operation, (op) => ResourceWriter(op, WriteCatlet));
            return;

            void WriteCatlet(ResourceType resourceType, string id)
            {
                if (resourceType == ResourceType.Catlet)
                    WriteObject(GetSingleCatlet(id));

            }
        }
    }
}