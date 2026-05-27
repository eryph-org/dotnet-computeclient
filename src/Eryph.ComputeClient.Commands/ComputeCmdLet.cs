using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Management.Automation;
using System.Net;
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

        [CanBeNull] private ApiVersionInfo _apiVersionInfo;

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

        protected ApiVersionInfo GetApiVersion()
        {
            if (_apiVersionInfo is not null)
                return _apiVersionInfo;

            try
            {
                var response = Factory.CreateVersionClient().Get();
                var version = response.Value.LatestVersion;
                _apiVersionInfo = new ApiVersionInfo(version.Major, version.Minor);
            }
            catch (RequestFailedException rfe) when (rfe.Status == (int)HttpStatusCode.NotFound)
            {
                // Fallback for versions of eryph which do not support the version endpoint yet
                _apiVersionInfo = new ApiVersionInfo(1, 0);
            }

            return _apiVersionInfo;
        }

        protected void RequireApiVersion(int major, int minor, string feature)
        {
            var version = GetApiVersion();
            if (version.IsCompatible(major, minor))
                return;

            var message = $"The feature '{feature}' requires eryph API version {major}.{minor} or higher, "
                          + $"but the server reports version {version.Major}.{version.Minor}. "
                          + "Please update the eryph server.";
            ThrowTerminatingError(new ErrorRecord(
                new InvalidOperationException(message),
                "ApiVersionNotSupported",
                ErrorCategory.InvalidOperation,
                null));
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
                _ when resourceType == ResourceType.CatletSpecification =>
                    Factory.CreateCatletSpecificationsClient().Get(id).Value,
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

        /// <summary>
        /// Determines whether a positional value should be treated as a resource id
        /// rather than a name. Resource ids are GUIDs; a value that parses as a GUID
        /// and contains no wildcard characters is an id. As resource names are short,
        /// restricted strings they can never collide with the GUID form.
        /// </summary>
        protected static bool IsResourceId(string value) =>
            !string.IsNullOrWhiteSpace(value)
            && !WildcardPattern.ContainsWildcardCharacters(value)
            && Guid.TryParse(value, out _);

        /// <summary>
        /// Resolves a value that may be either a resource id (a GUID) or a name pattern
        /// into the matching resources. A GUID is looked up by id; otherwise the listing
        /// is filtered by name. Following the convention of cmdlets like Get-Process, an
        /// exact name (no wildcards) that matches nothing produces a not-found error,
        /// while a wildcard pattern simply yields nothing. Used both to write results
        /// (Get-*) and to resolve action targets (Start-/Stop-/Remove-*).
        /// </summary>
        protected IEnumerable<T> ResolveByNameOrId<T>(
            string nameOrId,
            Func<string, T> getById,
            Func<IEnumerable<T>> listFactory,
            Func<T, string> nameSelector,
            string resourceKind)
        {
            if (IsResourceId(nameOrId))
            {
                if (TryGetById(nameOrId, getById, resourceKind, out var item))
                    yield return item;
                yield break;
            }

            foreach (var item in FilterByName(listFactory(), nameOrId, nameSelector, resourceKind))
                yield return item;
        }

        /// <summary>
        /// Filters a listing by a name pattern (PowerShell wildcards, case-insensitive).
        /// An exact name (no wildcards) that matches nothing produces a not-found error;
        /// a wildcard pattern yields nothing; a null/empty pattern yields everything.
        /// As eryph has no server-side search, the filtering is performed client-side.
        /// </summary>
        protected IEnumerable<T> FilterByName<T>(
            IEnumerable<T> items,
            string namePattern,
            Func<T, string> nameSelector,
            string resourceKind)
        {
            var hasWildcards = !string.IsNullOrEmpty(namePattern)
                               && WildcardPattern.ContainsWildcardCharacters(namePattern);
            var pattern = string.IsNullOrWhiteSpace(namePattern)
                ? null
                : new WildcardPattern(namePattern, WildcardOptions.IgnoreCase);

            var matched = false;
            foreach (var item in items)
            {
                if (Stopping) yield break;

                if (pattern is not null)
                {
                    var name = nameSelector(item);
                    if (name is null || !pattern.IsMatch(name))
                        continue;
                }

                matched = true;
                yield return item;
            }

            // An exact name (no wildcards) that matches nothing is an error, mirroring
            // Get-Process/Get-Service. A wildcard pattern simply yields nothing. Do not
            // raise the error when the pipeline was stopped before a match was found.
            if (!Stopping && !matched && pattern is not null && !hasWildcards)
                WriteError(ResourceNotFound(resourceKind, "name", namePattern));
        }

        /// <summary>
        /// Resolves a name-or-id value and writes the matching resources to the pipeline.
        /// </summary>
        protected void WriteByNameOrId<T>(
            string nameOrId,
            Func<string, T> getById,
            Func<IEnumerable<T>> listFactory,
            Func<T, string> nameSelector,
            string resourceKind,
            Action<T> writeItem = null)
        {
            writeItem ??= item => WriteObject(item);
            foreach (var item in ResolveByNameOrId(nameOrId, getById, listFactory, nameSelector, resourceKind))
                writeItem(item);
        }

        /// <summary>
        /// Writes the items of a listing to the pipeline, filtered by a name pattern.
        /// </summary>
        protected void WriteFilteredByName<T>(
            IEnumerable<T> items,
            string namePattern,
            Func<T, string> nameSelector,
            string resourceKind,
            Action<T> writeItem = null)
        {
            writeItem ??= item => WriteObject(item);
            foreach (var item in FilterByName(items, namePattern, nameSelector, resourceKind))
                writeItem(item);
        }

        /// <summary>
        /// Looks up a resource by id, translating a 404 into a friendly non-terminating
        /// not-found error instead of leaking a <see cref="RequestFailedException"/>.
        /// </summary>
        protected bool TryGetById<T>(
            string id,
            Func<string, T> getById,
            string resourceKind,
            out T item)
        {
            try
            {
                item = getById(id);
                return true;
            }
            catch (RequestFailedException ex) when (ex.Status == (int)HttpStatusCode.NotFound)
            {
                WriteError(ResourceNotFound(resourceKind, "id", id));
                item = default;
                return false;
            }
        }

        protected static ErrorRecord ResourceNotFound(string resourceKind, string by, string value)
        {
            var errorId = string.Concat(resourceKind
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(word => char.ToUpperInvariant(word[0]) + word.Substring(1)));
            return new ErrorRecord(
                new ItemNotFoundException($"Cannot find a {resourceKind} with the {by} '{value}'."),
                $"{errorId}NotFound",
                ErrorCategory.ObjectNotFound,
                value);
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