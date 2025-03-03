using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Eryph.ComputeClient.Models;

public partial class ExpandNewCatletConfigRequest
{
    public JsonElement Configuration { get; set; }
}
