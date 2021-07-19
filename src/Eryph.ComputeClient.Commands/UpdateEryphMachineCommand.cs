using System;
using System.Management.Automation;
using Eryph.ClientRuntime;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands
{
    [PublicAPI]
    [Cmdlet(VerbsData.Update, "EryphMachine")]
    [OutputType(typeof(Operation), typeof(Machine), typeof(VirtualMachine))]
    public class UpdateEryphMachineCommand : MachineConfigCmdlet
    {
        [Parameter(
            Position = 0,
            ValueFromPipeline = true,
            Mandatory = true,
            ValueFromPipelineByPropertyName = true)]
        public string[] Id { get; set; }

        [Parameter]
        public string MachineConfig { get; set; }

        [Parameter]
        public SwitchParameter Wait
        {
            get => _wait;
            set => _wait = value;
        }
        
        private bool _wait;


        protected override void ProcessRecord()
        {
            foreach (var id in Id)
            {
                var config = DeserializeConfigString(MachineConfig);

                //WaitForOperation(ComputeClient.Machines.Update(id, Guid.NewGuid(), config)
                //    ,_wait, true);
            }

        }

    }
}