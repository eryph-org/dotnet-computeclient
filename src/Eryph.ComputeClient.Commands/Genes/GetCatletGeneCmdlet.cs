using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.Genes;

[PublicAPI]
[Cmdlet(VerbsCommon.Get, "CatletGene", DefaultParameterSetName = "get")]
[OutputType(typeof(GeneWithUsage), ParameterSetName = ["get"])]
[OutputType(typeof(Gene), ParameterSetName = ["list"])]
public class GetCatletGeneCmdlet : CatletGeneCmdlet
{
    [Parameter(
        ParameterSetName = "get",
        Position = 0,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true)]
    public string[] Id { get; set; }

    protected override void ProcessRecord()
    {
        if (Id != null)
        {
            foreach (var id in Id)
            {
                WriteObject(GetSingleGene(id));
            }

            return;
        }

        foreach (var gene in Factory.CreateGenesClient().List())
        {
            if (Stopping) break;
            WriteObject(gene, true);
        }
    }
}
