using System;
using System.Management.Automation;
using Eryph.ClientRuntime;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.Projects
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Remove, "EryphProject")]
    [OutputType(typeof(Operation))]
    public class RemoveProjectCommand : ProjectCmdlet
    {
        [Parameter(
            Position = 0,
            ValueFromPipeline = true,
            Mandatory = true,
            ValueFromPipelineByPropertyName = true)]
        public string[] Id { get; set; }

        [Parameter]
        public SwitchParameter NoWait
        {
            get => _nowait;
            set => _nowait = value;
        }

        private bool _nowait;


        protected override void ProcessRecord()
        {
            foreach (var id in Id)
            {
                WaitForProject(Factory.CreateProjectsClient().Delete(id)
                    , _nowait, false, id);
            }

        }

    }
}