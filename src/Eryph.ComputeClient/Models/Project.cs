using System;
using System.Collections.Generic;
using System.Text;

namespace Eryph.ComputeClient.Models;

public partial class Project
{
    public override string ToString() => $"{Name} ({Id})";
}
