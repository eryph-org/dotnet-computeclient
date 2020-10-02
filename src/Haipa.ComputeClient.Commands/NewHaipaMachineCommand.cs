using System.Management.Automation;
using Haipa.ClientRuntime;
using Haipa.ComputeClient.Models;
using JetBrains.Annotations;

namespace Haipa.ComputeClient.Commands
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.New, "HaipaMachine")]
    [OutputType(typeof(Operation))]
    public class NewHaipaMachineCommand : MachineConfigCmdlet
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

                WaitForOperation(ComputeClient.Machines.Create(config),_wait, true);
            }

        }


    }
}