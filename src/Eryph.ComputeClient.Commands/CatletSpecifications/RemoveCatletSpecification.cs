using System;
using System.Management.Automation;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.CatletSpecifications;

[PublicAPI]
[Cmdlet(VerbsCommon.Remove, "CatletSpecification")]
[OutputType(typeof(Operation), typeof(CatletSpecification))]
public class RemoveCatletSpecification : CatletSpecificationCmdlet
{
    [Parameter(
        Position = 0,
        ValueFromPipeline = true,
        Mandatory = true,
        ValueFromPipelineByPropertyName = true)]
    public string[] Id { get; set; }

    [Parameter]
    public SwitchParameter RemoveCatlet { get; set; }

    [Parameter]
    public SwitchParameter Force { get; set; }
    
    [Parameter]
    public SwitchParameter PassThru { get; set; }

    [Parameter]
    public SwitchParameter NoWait { get; set; }

    private bool _yesToAll;
    private bool _noToAll;

    protected override void ProcessRecord()
    {
        foreach (var id in Id)
        {
            CatletSpecification specification;
            try
            {
                specification = Factory.CreateCatletSpecificationsClient().Get(id);
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "CatletSpecificationNotFound", ErrorCategory.ObjectNotFound, id));
                continue;
            }

            if (!Force && !ShouldContinue($"Catlet specification '{specification.Name}' (Id:{id}) will be deleted!", "Warning!",
                    ref _yesToAll, ref _noToAll))
            {
                continue;
            }

            WaitForOperation(Factory.CreateCatletSpecificationsClient().Delete(
                    id,
                    new DeleteCatletSpecificationRequestBody
                    {
                        DeleteCatlet = RemoveCatlet,
                    }),
                NoWait,
                false,
                id);

            if (PassThru)
                WriteObject(specification);
        }
    }
}
