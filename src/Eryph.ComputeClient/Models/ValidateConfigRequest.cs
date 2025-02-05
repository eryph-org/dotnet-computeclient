using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Eryph.ComputeClient.Models
{
    public partial class ValidateConfigRequest
    {
        public JsonElement Configuration { get; set; }
    }
}
