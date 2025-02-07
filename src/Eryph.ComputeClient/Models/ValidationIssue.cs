using System;
using System.Collections.Generic;
using System.Text;

namespace Eryph.ComputeClient.Models
{
    public partial class ValidationIssue
    {
        public override string ToString() => $"{Member}: {Message}";
    }
}
