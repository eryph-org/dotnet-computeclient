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
    private readonly IDictionary<string, Activity> _activities = new Dictionary<string, Activity>();
    private readonly IDictionary<string, int> _activityIds = new Dictionary<string, int>();

    public OperationProgressManager(string operationId)
    {
        _operationId = operationId;
        _activityIds.Add(operationId, 0);
    }

    public void Update(Operation operation)
    {
        var tasksById = operation.Tasks.ToDictionary(t => t.Id);
        EnsureActivities(tasksById);
    }

    private void EnsureActivities(IDictionary<string, OperationTask> tasks)
    {
        int nextId = _activities.Count > 0 ? _activities.Values.Select(a => a.ActivityId).Max() + 1 : 0;
        foreach (var task in tasks.Values.Where(t => !string.IsNullOrWhiteSpace(t.DisplayName)))
        {
            if (!_activities.ContainsKey(task.Id))
            {
                _activities.Add(task.Id, new Activity()
                {
                    ActivityId = nextId,
                    TaskId = task.Id,
                });
                _activityIds.Add(task.Id, nextId);
                nextId++;
            }
        }
    }

    private sealed class Activity
    {
        public int ActivityId { get; set; }

        public string ActivityName { get; set; }

        public int ParentActivityId { get; set; } = -1;

        public string TaskId { get; set; }

        public ProgressRecordType RecordType { get; set; }

        public string LastLogMessage { get; set; }

        public DateTimeOffset LastLogTimestamp { get; set; }

        public int PercentComplete { get; set; }
    }
}