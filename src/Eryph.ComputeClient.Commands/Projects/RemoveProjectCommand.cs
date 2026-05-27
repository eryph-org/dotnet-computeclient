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
    [Alias("Name")]
    public string[] Id { get; set; }

    [Parameter]
    public SwitchParameter Force { get; set; }

    [Parameter]
    public SwitchParameter NoWait { get; set; }

    private bool _yesToAll;
    private bool _noToAll;

    protected override void ProcessRecord()
    {
        foreach (var nameOrId in Id)
        {
            foreach (var project in ResolveByNameOrId(nameOrId,
                         id => Factory.CreateProjectsClient().Get(id).Value,
                         () => Factory.CreateProjectsClient().List(),
                         p => p.Name, "project"))
            {
                if (Stopping) break;

                if (!Force && !ShouldContinue($"Project '{project.Name}' (Id:{project.Id}) and all catlets in the project will be deleted!", "Warning!",
                        ref _yesToAll, ref _noToAll))
                {
                    continue;
                }

                WaitForProject(Factory.CreateProjectsClient().Delete(project.Id), NoWait, false, project.Id);
            }
        }
    }
}
