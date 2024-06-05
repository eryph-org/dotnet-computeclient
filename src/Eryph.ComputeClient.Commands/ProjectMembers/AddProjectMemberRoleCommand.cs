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
        public Guid RoleId { get; set; }

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

            if (RoleId == Guid.Empty)
            {
                switch (Role)
                {
                    case BuildInRole.Owner:
                        RoleId = Guid.Parse("{918D2C23-8E9A-41AE-8F0E-ADACA3BECBC4}");
                        break;
                    case BuildInRole.Contributor:
                        RoleId = Guid.Parse("{6C526814-2466-4BC1-92FD-1728C1152F3D}");
                        break;
                    case BuildInRole.Reader:
                        RoleId = Guid.Parse("{47982531-A6D2-41E4-AD6C-D5023DEB7710}");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(Role));
                }
            }

            var recordId = Guid.NewGuid();
            WaitForMember(
                Factory.CreateProjectMembersClient().Add(
                    projectId.GetValueOrDefault(),
                    new NewProjectMemberBody(MemberId, RoleId)
                    {
                        CorrelationId = recordId,
                    }), 
                _nowait,
                true);
        }
    }
}
