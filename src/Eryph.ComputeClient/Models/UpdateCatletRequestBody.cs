using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Eryph.ComputeClient.Models;

public partial class UpdateCatletRequestBody
{
    public JsonElement Configuration { get; }
}
