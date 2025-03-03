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
            string knownMemberId = null,
            string knownProjectId = null)
        {
            if (noWait)
            {
                if (knownMemberId == null || knownProjectId == null || !preferWriteMember)
                    WriteObject(operation);
                else
                    WriteObject(Factory.CreateProjectMembersClient()
                        .Get(knownProjectId, knownMemberId).Value);
                return;
            }

            var completedOperation = WaitForOperation(operation);
            if (preferWriteMember)
            {
                WriteMember(completedOperation);
                return;
            }

            WriteObject(completedOperation);
        }

        private void WriteMember(Operation operation)
        {
            var operationWithTasks = Factory.CreateOperationsClient()
                .Get(operation.Id, expand: "tasks").Value;
            var memberData = operationWithTasks.Tasks
                .Where(x => x.Reference != null)
                .FirstOrDefault(x => x.Reference.Type == TaskReferenceType.ProjectMember);

            if (memberData == null)
                return;

            var projectId = GetProjectId(memberData.Reference.ProjectName);
            var projectMember = Factory.CreateProjectMembersClient()
                .Get(projectId, memberData.Reference.Id).Value;
            WriteObject(projectMember);
        }
    }
}