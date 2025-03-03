using System;
using System.Linq;
using Eryph.ComputeClient.Models;

namespace Eryph.ComputeClient.Commands.Projects
{
    public class ProjectCmdlet : ComputeCmdLet
    {
        protected void WaitForProject(
            Operation operation,
            bool noWait,
            bool preferWriteProject,
            string knownProjectId = null)
        {
            if (noWait)
            {
                if (knownProjectId == null || !preferWriteProject)
                    WriteObject(operation);
                else
                    WriteObject(Factory.CreateProjectsClient().Get(knownProjectId).Value);
                return;
            }

            var completedOperation = WaitForOperation(operation);

            if (preferWriteProject)
            {
                WriteProject(operation);
                return;
            }

            WriteObject(completedOperation);
        }


        private void WriteProject(Operation operation)
        {
            var operationWithProjects = Factory.CreateOperationsClient()
                .Get(operation.Id, expand: "projects").Value;
            var projectData = operationWithProjects.Projects.FirstOrDefault();
            if (projectData is null)
                return;
            
            var project = Factory.CreateProjectsClient().Get(projectData.Id).Value;
            WriteObject(project);
        }
    }
}