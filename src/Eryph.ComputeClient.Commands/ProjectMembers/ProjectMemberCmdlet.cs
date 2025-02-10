using System.Linq;
using System;
using Eryph.ComputeClient.Models;

namespace Eryph.ComputeClient.Commands.ProjectMembers
{
    public class ProjectMemberCmdlet : ComputeCmdLet
    {
        protected void WaitForMember(
            Operation operation,
            bool noWait, 
            bool preferWriteMember,
            string knownMemberId = default,
            string knownProjectId = default)
        {
            if (noWait)
            {
                if (knownMemberId == default || knownProjectId == default || !preferWriteMember)
                    WriteObject(operation);
                else
                    WriteObject(Factory.CreateProjectMembersClient()
                        .Get(knownProjectId, knownMemberId).Value);
                return;
            }

            WaitForOperation(operation, preferWriteMember ? WriteMember : WriteObject);
        }


        private void WriteMember(Operation operation)
        {
            
            var newOp = Factory.CreateOperationsClient().Get(operation.Id, 
                expand: "tasks").Value;
            var memberData = newOp.Tasks
                .Where(x => x.Reference != null)
                .FirstOrDefault(x => x.Reference.Type == TaskReferenceType.ProjectMember);
            if (memberData != null)
            {
                var projectId = GetProjectId(memberData.Reference.ProjectName);
                WriteObject(Factory.CreateProjectMembersClient()
                    .Get(projectId, memberData.Reference.Id).Value);
            }

        }
    }
}