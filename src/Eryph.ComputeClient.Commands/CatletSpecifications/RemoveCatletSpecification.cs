using System;
using System.Management.Automation;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.CatletSpecifications;

[PublicAPI]
[Cmdlet(VerbsCommon.Remove, "CatletSpecification", DefaultParameterSetName = "Wait")]
[OutputType(typeof(CatletSpecification), ParameterSetName = new[] { "Wait" })]
[OutputType(typeof(Operation), ParameterSetName = new[] { "NoWait" })]
public class RemoveCatletSpecification : CatletSpecificationCmdlet
{
    [Parameter(
        Position = 0,
        ValueFromPipeline = true,
        Mandatory = true,
        ValueFromPipelineByPropertyName = true)]
    [Alias("Name")]
    public string[] Id { get; set; }

    [Parameter]
    public SwitchParameter RemoveCatlet { get; set; }

    [Parameter]
    public SwitchParameter Force { get; set; }
    
    [Parameter]
    public SwitchParameter PassThru { get; set; }

    [Parameter(ParameterSetName = "NoWait")]
    public SwitchParameter NoWait { get; set; }

    [Parameter]
    [ValidateNotNullOrEmpty]
    public string ProjectName { get; set; }

    private bool _yesToAll;
    private bool _noToAll;

    protected override void ProcessRecord()
    {
        foreach (var nameOrId in Id)
        {
            foreach (var specification in ResolveActionTargets(nameOrId, ProjectName, GetSingleCatletSpecification,
                         projectId => Factory.CreateCatletSpecificationsClient().List(projectId: projectId),
                         s => s.Name, "catlet specification"))
            {
                if (Stopping) break;

                if (!Force && !ShouldContinue($"Catlet specification '{specification.Name}' (Id:{specification.Id}) will be deleted!", "Warning!",
                        ref _yesToAll, ref _noToAll))
                {
                    continue;
                }

                WaitForOperation(Factory.CreateCatletSpecificationsClient().Delete(
                        specification.Id,
                        new DeleteCatletSpecificationRequestBody
                        {
                            DeleteCatlet = RemoveCatlet,
                        }),
                    NoWait,
                    false,
                    specification.Id);

                if (PassThru)
                    WriteObject(specification);
            }
        }
    }
}
