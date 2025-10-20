using System;
using System.Collections.Generic;
using System.Text;

namespace Eryph.ComputeClient.Models;

public partial class CatletSpecificationVersionInfo
{

    public override string ToString()
    {
        if (string.IsNullOrWhiteSpace(Comment))
            return $"{CreatedAt}";

        return Comment.Length <= 20 ? $"{CreatedAt} ({Comment})" : $"{CreatedAt} ({Comment.Substring(0, 19)}…)";
    }
}
