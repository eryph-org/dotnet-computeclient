using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Eryph.ComputeClient.Models;

public partial class CatletSpecificationVersionGene
{
    public override string ToString() =>
        $"{GeneType}::gene:{GeneSet}:{Name}[{Architecture}]".ToLowerInvariant();
}
