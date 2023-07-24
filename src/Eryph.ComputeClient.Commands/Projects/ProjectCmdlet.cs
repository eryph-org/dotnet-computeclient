using System;
using System.Linq;
using Eryph.ComputeClient.Models;

namespace Eryph.ComputeClient.Commands.Projects
{
    public class ProjectCmdlet : ComputeCmdLet
    {
        protected void WaitForProject(Operation operation, bool wait, bool preferWriteProject, string knownProjectId = default)
        {
            if (!wait)
            {
                if (knownProjectId == default || !preferWriteProject)
                    WriteObject(operation);
                else
                    WriteObject(Factory.CreateProjectsClient().Get(knownProjectId).Value);
                return;
            }

            WaitForOperation(operation, preferWriteProject ? (Action<Operation>)WriteProject : null);
        }


        private void WriteProject(Operation operation)
        {
            var newOp = Factory.CreateOperationsClient().Get(operation.Id, expand: "projects").Value;
            var projectData = newOp.Projects.FirstOrDefault();
            if (projectData != null)
            {
                WriteObject(Factory.CreateProjectsClient().Get(projectData.Id).Value);
            }

        }
    }
}