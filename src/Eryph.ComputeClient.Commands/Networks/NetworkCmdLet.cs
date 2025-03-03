using Eryph.ComputeClient.Models;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.Networks
{
    [PublicAPI]
    public abstract class NetworkCmdLet : ComputeCmdLet
    {
        protected VirtualNetwork GetSingleNetwork(string id)
        {
            return Factory.CreateVirtualNetworksClient().Get(id);
        }

        protected void WaitForOperation(
            Operation operation,
            bool noWait,
            bool alwaysWriteNetwork,
            string knownNetworkId = null)
        {
            if (noWait)
            {
                if (knownNetworkId == null || !alwaysWriteNetwork)
                    WriteObject(operation);
                else
                    WriteObject(GetSingleNetwork(knownNetworkId));
                return;
            }

            var completedOperation = WaitForOperation(operation);
            WriteResources(completedOperation, ResourceType.VirtualNetwork);
        }
    }
}