using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using Azure;
using Eryph.ClientRuntime.Configuration;
using Eryph.ClientRuntime.Powershell;
using Eryph.ComputeClient.Commands.Catlets;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;
using Operation = Eryph.ComputeClient.Models.Operation;

namespace Eryph.ComputeClient.Commands
{
    [PublicAPI]
    public abstract class ComputeCmdLet : EryphCmdLet
    {
        protected ComputeClientsFactory Factory;

        protected bool IsDebugEnabled
        {
            get
            {
                bool debug;
                var containsDebug = MyInvocation.BoundParameters.ContainsKey("Debug");
                if (containsDebug)
                    debug = ((SwitchParameter)MyInvocation.BoundParameters["Debug"]).ToBool();
                else
                    debug = (ActionPreference)GetVariableValue("DebugPreference") != ActionPreference.SilentlyContinue;

                return debug;
            }
        }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            var options = new EryphComputeClientOptions(GetClientCredentials())
            {
                Diagnostics =
                {
                    IsDistributedTracingEnabled = true,
                    IsLoggingEnabled = IsDebugEnabled,
                    IsLoggingContentEnabled = IsDebugEnabled,
                    LoggedHeaderNames = { "api-supported-versions" },
                }
            };


            Factory = new ComputeClientsFactory(
                options, GetEndpointUri("compute"));

        }


        protected Uri GetEndpointUri(string endpoint)
        {

            var credentials = GetClientCredentials();
            var endpointLookup = new EndpointLookup(new PowershellEnvironment(SessionState));
            return endpointLookup.GetEndpoint(endpoint, credentials.Configuration);

        }

        protected void WriteResources(Operation operation, ResourceType resourceType)
        {
            var resourceData = Factory.CreateOperationsClient()
                .Get(operation.Id, expand: "resources").Value;

            var resourceIds = resourceData.Resources
                .Where(r => r.ResourceType == resourceType && !string.IsNullOrWhiteSpace(r.ResourceId))
                .Select(r => r.ResourceId)
                .ToList();

            try
            {
                foreach (var resourceId in resourceIds)
                {
                    WriteResource(resourceType, resourceId);
                }
            }
            catch
            {
                WriteObject(resourceData);
            }

        }

        protected void WriteResource(ResourceType resourceType, string id)
        {
            object resource = resourceType switch
            {
                _ when resourceType == ResourceType.Catlet =>
                    Factory.CreateCatletsClient().Get(id).Value,
                _ when resourceType == ResourceType.VirtualDisk =>
                    Factory.CreateVirtualDisksClient().Get(id).Value,
                _ when resourceType == ResourceType.VirtualNetwork =>
                    Factory.CreateVirtualNetworksClient().Get(id).Value,
                _ => throw new NotSupportedException($"The resource type {resourceType} is not supported")
            };

            WriteObject(resource);
        }

        protected string GetProjectId(string projectName)
        {
            if (string.IsNullOrWhiteSpace(projectName))
                return null;

            var project = Factory.CreateProjectsClient().List().FirstOrDefault(x => x.Name == projectName);
            return project == null ? throw new ProjectNotFoundException(projectName) : project.Id;
        }

        protected void ListOutput<T>(Pageable<T> pageable)
        {
            foreach (var page in pageable)
            {
                if (Stopping) break;
                WriteObject(page, true);
            }
        }

        protected Operation WaitForOperation(Operation operation)
        {
            var timeStamp = DateTime.Parse("2018-01-01", CultureInfo.InvariantCulture);


            var processedLogIds = new List<string>();
            var activityCounter = -1;
            var activityIds = new Dictionary<string, int> { { operation.Id, activityCounter++ } };

            var completedActivities = new List<int>();

            var operationsClient = Factory.CreateOperationsClient();
            var currentOperation = operation;
            while (!Stopping)
            {
                Task.Delay(1000).GetAwaiter().GetResult();

                currentOperation = operationsClient.Get(operation.Id, timeStamp, expand: "logs,tasks").Value;

                foreach (var operationTask in currentOperation.Tasks)
                {
                    if (!activityIds.ContainsKey(operationTask.Id))
                        activityIds.Add(operationTask.Id, activityCounter++);
                }

                var activities = new Dictionary<string,ProgressRecord>();
                foreach (var operationTask in currentOperation.Tasks)
                {
                    var displayName = operationTask.DisplayName;
                    if(string.IsNullOrWhiteSpace(displayName))
                    {
                        if (operationTask.ParentTaskId == operation.Id)
                        {
                            displayName = operationTask.Name;
                            if(displayName.EndsWith("Command"))
                                // ReSharper disable once ReplaceSubstringWithRangeIndexer
                                displayName = displayName.Substring(0, displayName.Length - 7);
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(displayName))
                    {
                        var activityName = operationTask.ParentTaskId == operation.Id
                            ? $"{displayName} (Operation: {operation.Id})"
                            : $"{displayName} (Task: {operationTask.Id})";

                        var progressRecord = new ProgressRecord(activityIds[operationTask.Id], activityName,
                            operationTask.Status.ToString())
                        {
                            PercentComplete = operationTask.Status == OperationTaskStatus.Completed
                                ? 100
                                : operationTask.Progress,
                            ParentActivityId = !string.IsNullOrWhiteSpace(operationTask.ParentTaskId)
                                ? activityIds[operationTask.ParentTaskId]
                                : 0,
                            RecordType = operationTask.Status == OperationTaskStatus.Completed
                                ? ProgressRecordType.Completed
                                : ProgressRecordType.Processing,
                        };

                        if (progressRecord.PercentComplete == 0)
                            progressRecord.PercentComplete = -1;


                        var lastTaskLogEntry = currentOperation.LogEntries.Where(x => x.TaskId == operationTask.Id)
                            .OrderByDescending(x => x.Timestamp)
                            .FirstOrDefault();

                        if (lastTaskLogEntry != null)
                            progressRecord.CurrentOperation = lastTaskLogEntry.Message;

                        activities.Add(operationTask.Id,progressRecord);
                    }
                }

                // second pass for tasks without display name - to show them as operation of parent task
                foreach (var operationTask in currentOperation.Tasks.Where(x => x.ParentTaskId != operation.Id))
                {
                    var displayName = operationTask.DisplayName;
                    if (!string.IsNullOrWhiteSpace(displayName))
                        continue;

                    var lastTaskLogEntry = currentOperation.LogEntries.Where(x => x.TaskId == operationTask.Id)
                        .OrderByDescending(x => x.Timestamp)
                        .FirstOrDefault();

                    if (lastTaskLogEntry == null) continue; // no log, so no need to update parent

                    var parentId = operationTask.ParentTaskId;
                    while (parentId!= null)
                    {
                        var parentTask = currentOperation.Tasks.FirstOrDefault(x => x.Id == parentId);
                        if(parentTask == null)
                            break;
                        
                        parentId = parentTask.ParentTaskId;

                        if (!activities.TryGetValue(parentTask.Id, out var activity)) continue;
                        activity.CurrentOperation = lastTaskLogEntry.Message;
                        break;

                    }

                }

                foreach (var progressRecord in activities.Values.OrderBy(x=>x.ActivityId))
                {
                    if(completedActivities.Contains(progressRecord.ActivityId))
                        continue;

                    //looking for running child - in that case keep parent status to running
                    var runningChild = activities.Any(x =>
                        x.Value.ParentActivityId == progressRecord.ActivityId &&
                        progressRecord.RecordType == ProgressRecordType.Processing);

                    if(runningChild)
                        progressRecord.RecordType = ProgressRecordType.Processing;

                    WriteProgress(progressRecord);

                    if (progressRecord.RecordType == ProgressRecordType.Completed)
                        completedActivities.Add(progressRecord.ActivityId);
                }

                foreach (var logEntry in currentOperation.LogEntries)
                {
                    if (processedLogIds.Contains(logEntry.Id))
                        continue;

                    processedLogIds.Add(logEntry.Id);

                    WriteVerbose($"Operation {currentOperation.Id}: {logEntry.Message}");
                    timeStamp = logEntry.Timestamp.DateTime;
                    Task.Delay(100).GetAwaiter().GetResult();
                }

                switch (currentOperation.Status.ToString())
                {
                    case "Queued":
                    case "Running":
                        continue;
                    case "Failed":
                        WriteError(new ErrorRecord(new Exception(currentOperation.StatusMessage),
                            "EryphOperationFailed", ErrorCategory.InvalidResult, operation.Id));
                        break;
                }

                break;
            }

            return currentOperation;
        }
    }
}