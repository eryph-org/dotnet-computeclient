using System;
using Haipa.ClientRuntime;
using Haipa.CommonClient.Commands;
using Haipa.ComputeClient.Models;
using JetBrains.Annotations;

namespace Haipa.ComputeClient.Commands
{
    [PublicAPI]
    public abstract class ComputeCmdLet : ApiCmdLet
    {
        protected HaipaComputeClient ComputeClient;

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            ComputeClient = new HaipaComputeClient(GetCredentials("compute_api"));

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            ComputeClient?.Dispose();
            ComputeClient = null;

        }

        protected Machine GetSingleMachine(Guid id)
        {
            return ComputeClient.Machines.Get(id, null, "vm,networks");
        }

        protected void WaitForOperation(Operation operation, bool wait, bool alwaysWriteMachine, Guid knownMachineId = default)
        {
            if (!wait)
            {
                if(knownMachineId == default || !alwaysWriteMachine)
                    WriteObject(operation);
                else
                    WriteObject(GetSingleMachine(knownMachineId));
                return;
            }

            void WriteMachine(string resourceName, Guid id)
            {
                if (resourceName == "Machine") WriteObject(GetSingleMachine(id));
            }

            WaitForOperation(operation, !alwaysWriteMachine ? null : (Action<string, Guid>) WriteMachine);
        }
    }
}