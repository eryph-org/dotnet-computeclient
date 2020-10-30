using System;
using System.Management.Automation;
using Haipa.ClientRuntime;
using Haipa.ComputeClient.Models;
using JetBrains.Annotations;

namespace Haipa.ComputeClient.Commands
{
    [PublicAPI]
    [Cmdlet(VerbsData.Update, "HaipaMachine")]
    [OutputType(typeof(Operation), typeof(Machine), typeof(VirtualMachine))]
    public class UpdateHaipaVMCommand : MachineConfigCmdlet
    {
        [Parameter(
            Position = 0,
            ValueFromPipeline = true,
            Mandatory = true,
            ValueFromPipelineByPropertyName = true)]
        public Guid[] Id { get; set; }

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

                WaitForOperation(ComputeClient.Machines.Update(id, null, null, new MachineProvisioningSettings
                {
                    Configuration = config,
                    CorrelationId = Guid.NewGuid(),
                }),_wait, true);
            }

        }

    }
}