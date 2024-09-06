using Eryph.ComputeClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eryph.ComputeClient.Commands.Genes;

public abstract class CatletGeneCmdlet : ComputeCmdLet
{
    protected GeneWithUsage GetSingleGene(string id)
    {
        return Factory.CreateGenesClient().Get(id);
    }
}
