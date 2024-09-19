using System;
using System.Management.Automation;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.Projects;

[PublicAPI]
[Cmdlet(VerbsCommon.Remove, "EryphProject")]
[OutputType(typeof(Operation))]
public class RemoveProjectCommand : ProjectCmdlet
{
    [Parameter(
        Position = 0,
        ValueFromPipeline = true,
        Mandatory = true,
        ValueFromPipelineByPropertyName = true)]
    public string[] Id { get; set; }

    [Parameter]
    public SwitchParameter Force { get; set; }

    [Parameter]
    public SwitchParameter NoWait { get; set; }

    private bool _yesToAll;
    private bool _noToAll;

    protected override void ProcessRecord()
    {
        foreach (var id in Id)
        {
            Project project;
            try
            {
                project = Factory.CreateProjectsClient().Get(id);
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "ProjectNotFound", ErrorCategory.ObjectNotFound, id));
                continue;
            }

            if (!Force && !ShouldContinue($"Project '{project.Name}' (Id:{id}) and all catlets in the project will be deleted!", "Warning!",
                    ref _yesToAll, ref _noToAll))
            {
                continue;
            }

            WaitForProject(Factory.CreateProjectsClient().Delete(id), NoWait, false, id);
        }
    }
}
