using System.Management.Automation;
using Eryph.ComputeClient.Commands.Projects;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.ProjectMembers
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Get, "EryphProjectMemberRole", DefaultParameterSetName = "get")]
    [OutputType(typeof(ProjectMemberRole))]
    public class GetProjectMemberRoleCommand : ProjectCmdlet
    {
        [Parameter(
            ParameterSetName = "get",
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string[] Id { get; set; }

        [Parameter(
            ParameterSetName = "get",
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string ProjectName { get; set; }

        protected override void ProcessRecord()
        {
            if (Id != null)
            {
                foreach (var id in Id)
                {
                    WriteObject(Factory.CreateProjectMembersClient().Get(ProjectName, id).Value);
                }

                return;
            }

            ListOutput(Factory.CreateProjectMembersClient().List(ProjectName));


        }


    }
}