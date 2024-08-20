using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using Eryph.ComputeClient.Models;

namespace Eryph.ComputeClient.Commands;

public class OperationProgressManager
{
    private readonly string _operationId;
    private readonly IDictionary<string, int> _activityIds = new Dictionary<string, int>();

    public OperationProgressManager(string operationId)
    {
        _operationId = operationId;
        _activityIds.Add(operationId, 0);
    }

    public void Update(Operation operation)
    {
        EnsureActivityIds(operation);
    }

    private void EnsureActivityIds(Operation operation)
    {
        int nextId  = _activityIds.Values.Max() + 1;
        foreach(var task in operation.Tasks)
        {
            if (!_activityIds.ContainsKey(task.Id))
            {
                _activityIds.Add(task.Id, nextId);
                nextId++;
            }
        }
    }
}