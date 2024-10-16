using System.Management.Automation;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.ProjectMembers
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Remove, "EryphProjectMemberRole")]
    [OutputType(typeof(Operation))]
    public class RemoveProjectMemberRoleCommand : ProjectMemberCmdlet
    {
        [Parameter(
            Position = 0,
            ValueFromPipeline = true,
            Mandatory = true,
            ValueFromPipelineByPropertyName = true)]
        public string[] Id { get; set; }


        [Parameter(
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string ProjectName { get; set; }

        [Parameter]
        public SwitchParameter NoWait
        {
            get => _nowait;
            set => _nowait = value;
        }

        private bool _nowait;


        protected override void ProcessRecord()
        {
            var projectId = GetProjectId(ProjectName);
            foreach (var id in Id)
            {
                WaitForMember(
                    Factory.CreateProjectMembersClient().Remove(projectId, id),
                    _nowait, false, id, projectId);
            }

        }

    }
}