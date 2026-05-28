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
[Cmdlet(VerbsCommon.Get, "CatletGene", DefaultParameterSetName = "list")]
[OutputType(typeof(GeneWithUsage), ParameterSetName = ["get"])]
[OutputType(typeof(Gene), typeof(GeneWithUsage), ParameterSetName = ["list"])]
public class GetCatletGeneCmdlet : CatletGeneCmdlet
{
    [Parameter(
        ParameterSetName = "get",
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true)]
    public string[] Id { get; set; }

    // GeneSet is the natural identifier of a gene (org/set/version reference);
    // Name is a sub-attribute within a geneset. So the positional filter binds to
    // GeneSet and Name is offered as a secondary, non-positional refinement.
    [Parameter(
        ParameterSetName = "list",
        Position = 0)]
    [ValidateNotNullOrEmpty]
    public string GeneSet { get; set; }

    [Parameter(ParameterSetName = "list")]
    [ValidateNotNullOrEmpty]
    public string Name { get; set; }

    [Parameter(ParameterSetName = "list")]
    [ValidateNotNullOrEmpty]
    public string Architecture { get; set; }

    protected override void ProcessRecord()
    {
        if (Id != null)
        {
            foreach (var id in Id)
            {
                if (Stopping) break;
                if (TryGetById(id, GetSingleGene, "gene", out var gene))
                    WriteObject(gene);
            }

            return;
        }

        // A positional GUID is a gene record id; look it up directly (returns the
        // richer GeneWithUsage), mirroring -Id.
        if (IsResourceId(GeneSet))
        {
            if (TryGetById(GeneSet, GetSingleGene, "gene", out var geneById))
                WriteObject(geneById);
            return;
        }

        // Genes list globally and are identified by GeneSet + Name + Architecture.
        // GeneSet supports wildcards (with the standard Get-Process miss-not-found
        // semantics for an exact pattern); Architecture and Name are case-insensitive
        // refinement filters.
        IEnumerable<Gene> genes = Factory.CreateGenesClient().List();
        if (!string.IsNullOrWhiteSpace(Architecture))
            genes = genes.Where(gene =>
                string.Equals(gene.Architecture, Architecture, StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrWhiteSpace(Name))
        {
            var namePattern = new WildcardPattern(Name, WildcardOptions.IgnoreCase);
            genes = genes.Where(gene => gene.Name is not null && namePattern.IsMatch(gene.Name));
        }

        WriteFilteredByName(genes, GeneSet, gene => gene.GeneSet, "gene", fieldName: "geneset");
    }
}
