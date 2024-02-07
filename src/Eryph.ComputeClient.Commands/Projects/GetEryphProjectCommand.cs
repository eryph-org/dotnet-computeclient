using System;
using System.Management.Automation;
using Eryph.ClientRuntime;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.Projects
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Get, "EryphProject", DefaultParameterSetName = "get")]
    [OutputType(typeof(Project))]
    public class GetEryphProjectCommand : ProjectCmdlet
    {
        [Parameter(
            ParameterSetName = "get",
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string[] Id { get; set; }

        protected override void ProcessRecord()
        {
            if (Id != null)
            {
                foreach (var id in Id)
                {
                    WriteObject(Factory.CreateProjectsClient().Get(id).Value);
                }

                return;
            }

            ListOutput(Factory.CreateProjectsClient().List());


        }


    }
}