using Eryph.ComputeClient.Models;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.Networks
{
    [PublicAPI]
    public abstract class NetworkCmdLet : ComputeCmdLet
    {
        protected VirtualNetwork GetSingleNetwork(string id)
        {
            return Factory.CreateVNetworksClient().Get(id);
        }


        protected void WaitForOperation(Operation operation, bool wait, bool alwaysWriteNetwork, string knownNetworkId = default)
        {
            if (!wait)
            {
                if (knownNetworkId == default || !alwaysWriteNetwork)
                    WriteObject(operation);
                else
                    WriteObject(GetSingleNetwork(knownNetworkId));
                return;
            }

            void Write(ResourceType resourceType, string id)
            {
                if (resourceType == ResourceType.VirtualNetwork)
                    WriteObject(GetSingleNetwork(id));

            }

            WaitForOperation(operation, (op) => ResourceWriter(op, Write));
        }
    }
}