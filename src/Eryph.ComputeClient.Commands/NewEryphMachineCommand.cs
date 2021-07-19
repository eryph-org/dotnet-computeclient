using System;
using System.Management.Automation;
using Eryph.ClientRuntime;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.New, "EryphMachine")]
    [OutputType(typeof(Operation), typeof(Machine), typeof(VirtualMachine))]
    public class NewEryphMachineCommand : MachineConfigCmdlet
    {
        [Parameter(
            Position = 0,
            ValueFromPipeline = true,
            Mandatory = true,
            ValueFromPipelineByPropertyName = true)]
        public string[] InputObject { get; set; }

        [Parameter]
        public SwitchParameter Wait
        {
            get => _wait;
            set => _wait = value;
        }

        private bool _wait;


        protected override void ProcessRecord()
        {
            foreach (var inputObject in InputObject)
            {
                var config = DeserializeConfigString(inputObject);

                WaitForOperation(ComputeClient.Machines.Create(new NewMachineRequest
                {
                    Configuration = config,
                    CorrelationId = Guid.NewGuid()
                }),_wait, true);
            }

        }


    }
}