using System;
using System.Management.Automation;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.Projects
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Set, "EryphProject")]
    [OutputType(typeof(Operation), typeof(Project))]
    public class SetProjectCommand : ProjectCmdlet
    {
        [Parameter(
            Position = 0,
            ValueFromPipeline = true,
            Mandatory = true,
            ValueFromPipelineByPropertyName = true)]
        public string Id { get; set; }

        [Parameter(
            Position = 1,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string Name { get; set; }

        [Parameter]
        public SwitchParameter NoWait
        {
            get => _nowait;
            set => _nowait = value;
        }

        private bool _nowait;

        protected override void ProcessRecord()
        {
            WaitForProject(
                Factory.CreateProjectsClient().Update(
                    Id,
                    new UpdateProjectBody(Guid.NewGuid(), Name)),
                _nowait,
                true,
                Id);
        }
    }
}
