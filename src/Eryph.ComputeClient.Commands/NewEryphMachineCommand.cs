using System;
using System.Linq;
using System.Management.Automation;
using System.Text;
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
        [AllowEmptyString]
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
            var sb = new StringBuilder();
            foreach (var input in InputObject)
            {
                sb.AppendLine($"{input}");

            }

            var config = DeserializeConfigString(sb.ToString());


            WaitForOperation(ComputeClient.Machines.Create(new NewMachineRequest
            {
                Configuration = ConfigModelToJObject(config),
                CorrelationId = Guid.NewGuid()
            }), _wait, true);


        }


    }



}