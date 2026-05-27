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

    [Parameter(
        ParameterSetName = "list",
        ValueFromPipelineByPropertyName = true)]
    [ValidateNotNullOrEmpty]
    public string Name { get; set; }

    [Parameter(
        ParameterSetName = "list",
        ValueFromPipelineByPropertyName = true)]
    [ValidateNotNullOrEmpty]
    public string Architecture { get; set; }

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

        // Genes list globally and are identified by GeneSet + Name + Architecture.
        // Architecture is an exact (case-insensitive) filter; Name supports wildcards.
        IEnumerable<Gene> genes = Factory.CreateGenesClient().List();
        if (!string.IsNullOrWhiteSpace(Architecture))
            genes = genes.Where(gene =>
                string.Equals(gene.Architecture, Architecture, StringComparison.OrdinalIgnoreCase));

        WriteFilteredByName(genes, Name, gene => gene.Name);
    }
}
