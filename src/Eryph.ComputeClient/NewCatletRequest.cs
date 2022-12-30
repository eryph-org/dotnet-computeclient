using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

// ReSharper disable once CheckNamespace
namespace Eryph.ComputeClient.Models
{
    public partial class NewCatletRequest
    {
        public JsonElement Configuration { get; }
    }

    public partial class UpdateCatletRequest
    {
        public JsonElement Configuration { get; }
    }
}
