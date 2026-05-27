using System;
using System.Management.Automation;
using Eryph.ClientRuntime;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.Projects
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Get, "EryphProject", DefaultParameterSetName = "list")]
    [OutputType(typeof(Project))]
    public class GetEryphProjectCommand : ProjectCmdlet
    {
        [Parameter(
            ParameterSetName = "get",
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string[] Id { get; set; }

        [Parameter(
            ParameterSetName = "list",
            Position = 0)]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        protected override void ProcessRecord()
        {
            if (Id != null)
            {
                foreach (var id in Id)
                {
                    if (Stopping) break;
                    if (TryGetById(id, i => Factory.CreateProjectsClient().Get(i).Value, "project", out var project))
                        WriteObject(project);
                }

                return;
            }

            WriteByNameOrId(
                Name,
                id => Factory.CreateProjectsClient().Get(id).Value,
                () => Factory.CreateProjectsClient().List(),
                project => project.Name,
                "project");
        }


    }
}