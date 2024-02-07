using System.Linq;
using System;
using Eryph.ComputeClient.Models;

namespace Eryph.ComputeClient.Commands.ProjectMembers
{
    public class ProjectMemberCmdlet : ComputeCmdLet
    {
        protected void WaitForMember(Operation operation, bool noWait, 
            bool preferWriteMember, string knownMemberId = default, string knownProject = default)
        {
            if (noWait)
            {
                if (knownMemberId == default || knownProject == default || !preferWriteMember)
                    WriteObject(operation);
                else
                    WriteObject(Factory.CreateProjectMembersClient()
                        .Get(knownProject, knownMemberId).Value);
                return;
            }

            WaitForOperation(operation, preferWriteMember ? (Action<Operation>)WriteMember : null);
        }


        private void WriteMember(Operation operation)
        {
            var newOp = Factory.CreateOperationsClient().Get(operation.Id, 
                expand: "tasks").Value;
            var memberData = newOp.Tasks
                .Where(x=>x.Reference!= null)
                .FirstOrDefault(x=>x.Reference.Type.GetValueOrDefault() == TaskReferenceType.ProjectMember);
            if (memberData != null)
            {
                WriteObject(Factory.CreateProjectMembersClient()
                    .Get(memberData.Reference.ProjectName, memberData.Reference.Id).Value);
            }

        }
    }
}