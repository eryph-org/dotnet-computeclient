using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Eryph.ComputeClient.Models;

public partial class CatletSpecificationVersionVariant
{
    public JsonElement BuiltConfig { get; set; }

    public override string ToString() => Architecture;
}
