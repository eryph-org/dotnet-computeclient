using System;
using System.Management.Automation;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.ProjectMembers
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Add, "EryphProjectMemberRole")]
    [OutputType(typeof(Operation), typeof(ProjectMemberRole))]
    public class AddProjectMemberRoleCommand : ProjectMemberCmdlet
    {
        [Parameter(
            Mandatory = true,
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string MemberId { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string ProjectName { get; set; }

        [Parameter(
            ParameterSetName = "RoleId",
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string RoleId { get; set; }

        [Parameter(
            ParameterSetName = "Role",
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public BuildInRole Role { get; set; }

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

            if (string.IsNullOrEmpty(RoleId))
            {
                RoleId = Role switch
                {
                    BuildInRole.Owner => "918d2c23-8e9a-41ae-8f0e-adaca3becbc4",
                    BuildInRole.Contributor => "6c526814-2466-4bc1-92fd-1728c1152f3d",
                    BuildInRole.Reader => "47982531-a6d2-41e4-ad6c-d5023deb7710",
                    _ => throw new PSArgumentOutOfRangeException("The Role is not supported", Role, nameof(Role)),
                };
            }

            var recordId = Guid.NewGuid();
            WaitForMember(
                Factory.CreateProjectMembersClient().Add(
                    projectId,
                    new NewProjectMemberBody(MemberId, RoleId)
                    {
                        CorrelationId = recordId,
                    }), 
                _nowait,
                true);
        }
    }
}
