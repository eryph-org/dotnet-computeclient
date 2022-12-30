using System;
using System.Management.Automation;
using System.Text;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.Projects
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.New, "EryphProject")]
    [OutputType(typeof(Operation), typeof(Project))]
    public class NewProjectCommand : ProjectCmdlet
    {
        [Parameter(
            Position = 0,
            ValueFromPipeline = true,
            Mandatory = true,
            ValueFromPipelineByPropertyName = true)]
        public string Name { get; set; }

        [Parameter]
        public SwitchParameter Wait
        {
            get => _wait;
            set => _wait = value;
        }

        private bool _wait;


        protected override void ProcessRecord()
        {
            WaitForProject(Factory.CreateProjectsClient()
                .Create(new NewProjectRequest(Name){CorrelationId = Guid.NewGuid()}), _wait, true);

        }


    }



}