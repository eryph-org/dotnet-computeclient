using System;
using System.Collections.Generic;
using System.Text;

namespace Eryph.ComputeClient.Models;

public partial class VirtualDiskGeneInfo
{
    public override string ToString() => $"gene:{GeneSet}:{Name} ({Architecture})";
}
