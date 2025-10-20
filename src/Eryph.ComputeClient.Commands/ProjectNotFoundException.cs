using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eryph.ComputeClient.Commands;

public class ProjectNotFoundException : Exception
{
    public ProjectNotFoundException(string project)
        : base($"Project {project} not found")
    {
    }
}
