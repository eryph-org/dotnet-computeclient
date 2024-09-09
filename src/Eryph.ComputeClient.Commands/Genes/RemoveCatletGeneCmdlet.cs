using Eryph.ComputeClient.Models;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace Eryph.ComputeClient.Commands.Genes;

[PublicAPI]
[Cmdlet(VerbsCommon.Remove, "CatletGene", DefaultParameterSetName = "id")]
[OutputType(typeof(Operation), typeof(GeneWithUsage), ParameterSetName = ["id"])]
[OutputType(typeof(Operation), ParameterSetName = ["unused"])]
public class RemoveCatletGeneCmdlet : CatletGeneCmdlet
{
    [Parameter(
        ParameterSetName = "id",
        Mandatory = true,
        Position = 0,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true)]
    public string[] Id { get; set; }

    [Parameter(
        ParameterSetName = "unused",
        Mandatory = true)]
    public SwitchParameter Unused { get; set; }

    [Parameter]
    public SwitchParameter Force { get; set; }

    [Parameter]
    public SwitchParameter PassThru { get; set; }

    [Parameter]
    public SwitchParameter NoWait { get; set; }

    private bool _yesToAll, _noToAll;

    protected override void ProcessRecord()
    {
        if (Unused)
        {
            if (!Force && !ShouldContinue("All unused genes will be deleted from the local gene pool!", "Warning!"))
            {
                return;
            }

            WaitForOperation(Factory.CreateGenesClient().Cleanup());
            return;
        }

        if (Id is null)
            return;
        
        foreach (var id in Id)
        {
            if (Stopping) break;

            GeneWithUsage gene;
            try
            {
                gene = GetSingleGene(id);
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "GeneNotFound", ErrorCategory.ObjectNotFound, id));
                continue;
            }

            if (!Force && !ShouldContinue($"Gene '{gene.Name}' (Id:{id}) will be deleted from the local gene pool!", "Warning!",
                    ref _yesToAll, ref _noToAll))
            {
                continue;
            }

            WaitForOperation(Factory.CreateGenesClient().Delete(id).Value);
        }
    }
}
