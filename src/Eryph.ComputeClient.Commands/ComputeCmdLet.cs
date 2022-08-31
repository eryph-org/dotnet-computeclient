using System;
using Eryph.ClientRuntime;
using Eryph.CommonClient.Commands;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands
{
    [PublicAPI]
    public abstract class ComputeCmdLet : ApiCmdLet
    {
        protected EryphComputeClient ComputeClient;

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            ComputeClient = new EryphComputeClient(GetEndpointUri("compute"),
                GetCredentials("compute_api"));

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            ComputeClient?.Dispose();
            ComputeClient = null;

        }

        protected Machine GetSingleMachine(string id)
        {
            return ComputeClient.Machines.Get(id);
        }

        protected VirtualMachine GetSingleVM(string id)
        {
            return ComputeClient.VirtualMachines.Get(id);
        }


        protected void WaitForOperation(Operation operation, bool wait, bool alwaysWriteMachine, string knownMachineId = default)
        {
            if (!wait)
            {
                if(knownMachineId == default || !alwaysWriteMachine)
                    WriteObject(operation);
                else
                    WriteObject(GetSingleMachine(knownMachineId));
                return;
            }

            void WriteMachine(string resourceName, string id)
            {
                if (resourceName == "Machine") WriteObject(GetSingleMachine(id));
            }

            WaitForOperation(operation, !alwaysWriteMachine ? null : (Action<string, string>) WriteMachine);
        }
    }
}