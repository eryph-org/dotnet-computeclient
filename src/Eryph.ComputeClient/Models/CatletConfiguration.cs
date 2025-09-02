using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Eryph.ComputeClient.Models;

public partial class CatletConfiguration
{
    public JsonElement Configuration { get; }

    public JsonElement DeployedConfig { get; }
}
