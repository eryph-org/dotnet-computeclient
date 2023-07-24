using System;
using System.Management.Automation;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.Operations
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Get, "EryphOperation", DefaultParameterSetName = "get")]
    [OutputType(typeof(Operation))]
    public class GetEryphOperationCommand : ComputeCmdLet
    {
        [Parameter(
            ParameterSetName = "get",
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string[] Id { get; set; }

        [Parameter]
        public SwitchParameter WithLogs { get; set; }

        [Parameter]
        public SwitchParameter WithResources { get; set; }

        [Parameter]
        public SwitchParameter WithTasks { get; set; }

        [Parameter]
        public SwitchParameter WithProjects { get; set; }


        protected override void ProcessRecord()
        {
            var expandString = "";
            if (WithLogs)
                expandString += ",logs";
            if (WithResources)
                expandString += ",resources";
            if (WithProjects)
                expandString += ",projects";
            if (WithTasks)
                expandString += ",tasks";

            expandString = expandString.TrimStart(',');

            if (Id != null)
            {
                foreach (var id in Id)
                {
                    WriteObject(Factory.CreateOperationsClient().Get(id,expand: expandString).Value);
                }

                return;
            }

            ListOutput(Factory.CreateOperationsClient().List(expand: expandString));


        }


    }
}