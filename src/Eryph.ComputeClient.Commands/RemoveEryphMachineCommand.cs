using System;
using System.Management.Automation;
using Eryph.ClientRuntime;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Remove, "EryphMachine")]
    [OutputType(typeof(Operation), typeof(Machine))]
    public class RemoveEryphMachineCommand : ComputeCmdLet
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
        public SwitchParameter Force
        {
            get => _force;
            set => _force = value;
        }

        /// <summary>
        /// This parameter indicates that the cmdlet should return
        /// an object to the pipeline after the processing has been
        /// completed.
        /// </summary>
        [Parameter]
        public SwitchParameter PassThru
        {
            get => _passThru;
            set => _passThru = value;
        }

        [Parameter]
        public SwitchParameter Wait
        {
            get => _wait;
            set => _wait = value;
        }

        private bool _force;
        private bool _wait;
        private bool _passThru;

        private bool _yesToAll, _noToAll;



        protected override void ProcessRecord()
        {
            foreach (var id in Id)
            {
                Machine machine;
                try
                {
                    machine = ComputeClient.Machines.Get(id);
                }
                catch (ApiServiceException ex)
                {
                    WriteError(new ErrorRecord(ex, "MachineNotFound", ErrorCategory.ObjectNotFound, id));
                    continue;
                }

                if (!Force && !ShouldContinue($"Machine '{machine.Name}' (Id:{id}) will be deleted!", "Warning!",
                    ref _yesToAll, ref _noToAll))
                {
                    continue;
                }
                
                WaitForOperation(ComputeClient.Machines.Delete(id), _wait, _passThru, id);

                if (PassThru)
                    WriteObject(machine);
            }

        }

    }
}