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

        private readonly Dictionary<string, string> _projectIdCache = new();

        protected string GetProjectId(string projectName)
        {
            if (string.IsNullOrWhiteSpace(projectName))
                return null;

            if (_projectIdCache.TryGetValue(projectName, out var cachedId))
                return cachedId;

            var project = Factory.CreateProjectsClient().List().FirstOrDefault(x => x.Name == projectName);
            if (project == null)
                throw new ProjectNotFoundException(projectName);

            _projectIdCache[projectName] = project.Id;
            return project.Id;
        }

        /// <summary>
        /// Resolves a project name to its id, turning an unknown/inaccessible project into
        /// a non-terminating error (returning false) instead of a terminating exception.
        /// A null/empty name yields a null id and true (i.e. "no project filter").
        /// </summary>
        protected bool TryGetProjectId(string projectName, out string projectId)
        {
            try
            {
                projectId = GetProjectId(projectName);
                return true;
            }
            catch (ProjectNotFoundException ex)
            {
                WriteError(new ErrorRecord(ex, "ProjectNotFound", ErrorCategory.ObjectNotFound, projectName));
                projectId = null;
                return false;
            }
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
        /// <remarks>
        /// This is a deferred iterator that calls <c>WriteError</c> while enumerating, so
        /// it must be consumed synchronously on the pipeline thread (a foreach inside
        /// ProcessRecord/EndProcessing). Do not capture the result and enumerate it later.
        /// </remarks>
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
        /// Resolves the target(s) of a mutating cmdlet from a value that may be a resource
        /// id (a GUID) or a name. An id is used directly. A name must be scoped to a
        /// project (<paramref name="projectName"/> is required) so that it cannot fan out
        /// across projects; within the project a wildcard may still match several
        /// resources. This keeps destructive operations from acting on same-named
        /// resources in other projects.
        /// </summary>
        /// <remarks>
        /// Like <see cref="ResolveByNameOrId{T}"/> this is a deferred iterator that calls
        /// <c>WriteError</c> while enumerating; consume it on the pipeline thread.
        /// </remarks>
        protected IEnumerable<T> ResolveActionTargets<T>(
            string nameOrId,
            string projectName,
            Func<string, T> getById,
            Func<string, IEnumerable<T>> listInProject,
            Func<T, string> nameSelector,
            string resourceKind,
            string ambiguityHint = null)
        {
            if (IsResourceId(nameOrId))
            {
                if (TryGetById(nameOrId, getById, resourceKind, out var item))
                    yield return item;
                yield break;
            }

            if (string.IsNullOrWhiteSpace(projectName))
            {
                WriteError(new ErrorRecord(
                    new PSArgumentException(
                        $"When a {resourceKind} is identified by name, -ProjectName is required "
                        + "so the name can be resolved within a single project. Specify -ProjectName, "
                        + "or pass the id instead."),
                    "ProjectNameRequiredForNameLookup",
                    ErrorCategory.InvalidArgument,
                    nameOrId));
                yield break;
            }

            if (!TryGetProjectId(projectName, out var projectId))
                yield break;

            // A wildcard may intentionally match several resources. An exact name, however,
            // must resolve to a single target before a mutation: some resource names are
            // unique only per project + environment, so an exact name could otherwise match
            // several resources and the command would act on all of them.
            if (WildcardPattern.ContainsWildcardCharacters(nameOrId))
            {
                foreach (var item in FilterByName(listInProject(projectId), nameOrId, nameSelector, resourceKind))
                    yield return item;
                yield break;
            }

            var matches = FilterByName(listInProject(projectId), nameOrId, nameSelector, resourceKind).ToList();
            if (matches.Count > 1)
            {
                var hint = string.IsNullOrWhiteSpace(ambiguityHint)
                    ? "Specify the id to select one."
                    : $"Narrow the selection with {ambiguityHint} or specify the id.";
                WriteError(new ErrorRecord(
                    new PSArgumentException(
                        $"The {resourceKind} name '{nameOrId}' is ambiguous in project '{projectName}': "
                        + $"it matches {matches.Count} resources. {hint}"),
                    "AmbiguousNameInProject",
                    ErrorCategory.InvalidArgument,
                    nameOrId));
                yield break;
            }

            // 0 matches: FilterByName already emitted the not-found error during enumeration.
            foreach (var item in matches)
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
            string resourceKind,
            string fieldName = "name")
        {
            // Only a null/empty pattern means "no filter" (yield everything). A
            // whitespace-only value is a real (if non-matching) name, not "list all" —
            // ValidateNotNullOrEmpty lets ' ' through, so treat it as a name to match.
            var hasWildcards = !string.IsNullOrEmpty(namePattern)
                               && WildcardPattern.ContainsWildcardCharacters(namePattern);
            var pattern = string.IsNullOrEmpty(namePattern)
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

            // An exact value (no wildcards) that matches nothing is an error, mirroring
            // Get-Process/Get-Service. A wildcard pattern simply yields nothing. Do not
            // raise the error when the pipeline was stopped before a match was found.
            // `fieldName` lets callers that filter by something other than the resource's
            // `name` property (e.g. a gene's `geneset`) produce a meaningful error.
            if (!Stopping && !matched && pattern is not null && !hasWildcards)
                WriteError(ResourceNotFound(resourceKind, fieldName, namePattern));
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
            Action<T> writeItem = null,
            string fieldName = "name")
        {
            writeItem ??= item => WriteObject(item);
            foreach (var item in FilterByName(items, namePattern, nameSelector, resourceKind, fieldName))
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
            catch (RequestFailedException ex)
            {
                // 404 becomes a friendly not-found; other server errors (e.g. 403, 500)
                // are still surfaced as non-terminating so a loop over several ids can
                // continue, mirroring the previous behaviour.
                WriteError(ex.Status == (int)HttpStatusCode.NotFound
                    ? ResourceNotFound(resourceKind, "id", id)
                    : new ErrorRecord(ex, "RequestFailed", ErrorCategory.NotSpecified, id));
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


            var processedLogIds = new HashSet<string>();

            // The operation is shown as a single, stable master progress bar. It
            // carries no percentage: the number of tasks is only known at runtime and
            // most tasks never report progress, so any overall total would be
            // misleading. Only tasks that actually report progress get a child bar,
            // and each child bar is removed again as soon as its task stops reporting
            // progress - the set of progress-bearing tasks changes constantly (e.g.
            // parallel downloads/extractions), so keeping finished ones would quickly
            // fill the screen. A stable master plus few, real child bars is what
            // avoids the flickering described in #78.
            const int operationActivityId = 0;
            var taskActivityIds = new Dictionary<string, int>();
            var nextTaskActivityId = 1;
            var childRecords = new Dictionary<int, ProgressRecord>();
            var lastWrittenSignatures = new Dictionary<int, string>();

            void WriteProgressIfChanged(ProgressRecord record)
            {
                var signature = GetProgressSignature(record);
                if (lastWrittenSignatures.TryGetValue(record.ActivityId, out var previous)
                    && previous == signature)
                    return;

                lastWrittenSignatures[record.ActivityId] = signature;
                WriteProgress(record);
            }

            var operationsClient = Factory.CreateOperationsClient();
            var currentOperation = operation;
            ProgressRecord masterRecord = null;
            // Logs are fetched incrementally (the timeStamp advances each poll), so a
            // poll without new logs returns none. Remember the last phase/message so
            // quiet polls keep showing them instead of resetting the master bar.
            string lastPhase = null;
            string lastMessage = null;
            // Detect the progress view mode once; users do not change it mid-operation.
            // In Minimal view (PS7 default) only one line is shown for all activities,
            // so the master must carry the rich log message. In Classic view the master
            // and child render as separate stacked records, so we keep the master
            // concise when a child is already showing the detail.
            var isClassicProgressView = IsClassicProgressView();
            while (!Stopping)
            {
                Task.Delay(1000).GetAwaiter().GetResult();

                currentOperation = operationsClient.Get(operation.Id, timeStamp, expand: "logs,tasks").Value;

                var latestLog = currentOperation.LogEntries
                    .OrderByDescending(x => x.Timestamp)
                    .FirstOrDefault();

                // The master status text reflects the current phase: the task the
                // most recent log entry belongs to. On polls with new logs, refresh
                // the remembered phase/message; otherwise keep the previous ones so a
                // quiet poll does not blank the master bar.
                if (latestLog != null)
                {
                    lastMessage = latestLog.Message;
                    // Log entries usually come from deep sub-tasks that carry no
                    // DisplayName themselves. Walk up the parent chain to find the
                    // nearest task with a usable display name and use that as the
                    // current phase. Without this walk the master phase rarely gets
                    // updated and stays stuck on the operation status (see #78).
                    var logTask = currentOperation.Tasks.FirstOrDefault(x => x.Id == latestLog.TaskId);
                    // Track visited ids so an unexpected cycle in the task graph
                    // cannot loop forever.
                    var visitedTasks = new HashSet<string>(StringComparer.Ordinal);
                    while (logTask != null && visitedTasks.Add(logTask.Id))
                    {
                        var name = GetTaskDisplayName(logTask, operation.Id);
                        if (!string.IsNullOrWhiteSpace(name))
                        {
                            lastPhase = name;
                            break;
                        }
                        if (string.IsNullOrWhiteSpace(logTask.ParentTaskId))
                            break;
                        logTask = currentOperation.Tasks.FirstOrDefault(x => x.Id == logTask.ParentTaskId);
                    }
                }

                // Master status text:
                // - Minimal view (PS7 default): only one line is shown for the whole
                //   progress region, so the master MUST carry the rich log message -
                //   otherwise the user sees nothing informative during long phases.
                // - Classic view, child bar active: the child renders its own Activity
                //   and CurrentOperation lines and carries the message detail. Using
                //   the full message on the master too would duplicate the child's
                //   CurrentOperation line. Use the concise climbed phase instead.
                // - Classic view, no child active: master is the only thing on screen
                //   for these short config steps, so it carries the log message.
                // Falls back to the climbed phase, then to the operation status.
                var hasActiveChild = currentOperation.Tasks
                    .Any(x => x.Status == OperationTaskStatus.Running && x.Progress > 0);

                string statusText;
                if (isClassicProgressView && hasActiveChild && !string.IsNullOrWhiteSpace(lastPhase))
                {
                    statusText = lastPhase;
                }
                else
                {
                    statusText = !string.IsNullOrWhiteSpace(lastMessage)
                        ? lastMessage
                        : !string.IsNullOrWhiteSpace(lastPhase)
                            ? lastPhase
                            : currentOperation.Status.ToString();
                }
                if (string.IsNullOrWhiteSpace(statusText))
                    statusText = "Running"; // ProgressRecord requires a non-empty status description

                var primaryTask = currentOperation.Tasks
                    .Where(x => x.ParentTaskId == operation.Id)
                    .OrderBy(x => x.Id)
                    .FirstOrDefault();
                var operationName = primaryTask != null
                    ? GetTaskDisplayName(primaryTask, operation.Id)
                    : null;
                if (string.IsNullOrWhiteSpace(operationName))
                    operationName = "Operation";

                masterRecord = new ProgressRecord(operationActivityId,
                    $"{operationName} (Operation: {operation.Id})", statusText)
                {
                    PercentComplete = -1,
                    RecordType = ProgressRecordType.Processing,
                    // No CurrentOperation: in classic view this would render as an
                    // extra line below the master bar duplicating what an active
                    // child bar already shows in its own Activity line.
                };
                WriteProgressIfChanged(masterRecord);

                // Child bars are shown only for tasks that are currently running and
                // report a progress value.
                var activeChildIds = new HashSet<int>();
                foreach (var operationTask in currentOperation.Tasks
                             .Where(x => x.Status == OperationTaskStatus.Running && x.Progress > 0))
                {
                    if (!taskActivityIds.TryGetValue(operationTask.Id, out var activityId))
                    {
                        activityId = nextTaskActivityId++;
                        taskActivityIds[operationTask.Id] = activityId;
                    }

                    var taskName = GetTaskDisplayName(operationTask, operation.Id);
                    if (string.IsNullOrWhiteSpace(taskName))
                        taskName = "Task";

                    var progressRecord = new ProgressRecord(activityId, taskName, $"{operationTask.Progress}%")
                    {
                        PercentComplete = operationTask.Progress,
                        ParentActivityId = operationActivityId,
                        RecordType = ProgressRecordType.Processing,
                    };

                    var lastTaskLogEntry = currentOperation.LogEntries
                        .Where(x => x.TaskId == operationTask.Id)
                        .OrderByDescending(x => x.Timestamp)
                        .FirstOrDefault();
                    if (lastTaskLogEntry != null)
                        progressRecord.CurrentOperation = lastTaskLogEntry.Message;

                    childRecords[activityId] = progressRecord;
                    activeChildIds.Add(activityId);
                    WriteProgressIfChanged(progressRecord);
                }

                // Remove child bars whose task is no longer running with progress.
                foreach (var staleId in childRecords.Keys.Except(activeChildIds).ToList())
                {
                    var staleRecord = childRecords[staleId];
                    staleRecord.RecordType = ProgressRecordType.Completed;
                    WriteProgress(staleRecord);

                    childRecords.Remove(staleId);
                    lastWrittenSignatures.Remove(staleId);
                }

                // Iterate logs in chronological order so timeStamp always advances
                // to the newest entry in the batch. Otherwise a later poll could
                // re-request entries that fell between the last-iterated and the
                // max timestamp of this batch (still deduped by processedLogIds,
                // but wasted bandwidth).
                foreach (var logEntry in currentOperation.LogEntries.OrderBy(x => x.Timestamp))
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

                // The operation reached a terminal state. Complete the child bars
                // first, then the master, so PowerShell clears them cleanly.
                foreach (var record in childRecords.Values)
                {
                    record.RecordType = ProgressRecordType.Completed;
                    WriteProgress(record);
                }

                if (masterRecord != null)
                {
                    masterRecord.RecordType = ProgressRecordType.Completed;
                    WriteProgress(masterRecord);
                }

                break;
            }

            return currentOperation;
        }

        // PowerShell 7+ exposes the progress view mode via $PSStyle.Progress.View
        // ('Classic' or 'Minimal'). PowerShell 5.1 has no PSStyle and only renders
        // classic-style. Reflection avoids a hard reference to PS7-only types.
        // Returns true for Classic / PS5.1, false for Minimal.
        private bool IsClassicProgressView()
        {
            try
            {
                var psStyle = GetVariableValue("PSStyle");
                if (psStyle == null)
                    return true;

                var progress = psStyle.GetType().GetProperty("Progress")?.GetValue(psStyle);
                if (progress == null)
                    return true;

                var view = progress.GetType().GetProperty("View")?.GetValue(progress);
                return view == null
                       || string.Equals(view.ToString(), "Classic", StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return true;
            }
        }

        private static string GetTaskDisplayName(OperationTask task, string operationId)
        {
            var displayName = task.DisplayName;
            if (string.IsNullOrWhiteSpace(displayName) && task.ParentTaskId == operationId)
            {
                displayName = task.Name;
                if (!string.IsNullOrWhiteSpace(displayName) && displayName.EndsWith("Command"))
                    // ReSharper disable once ReplaceSubstringWithRangeIndexer
                    displayName = displayName.Substring(0, displayName.Length - 7);
            }

            return displayName;
        }

        // Field separator for progress signatures: a control character that cannot
        // occur in user-visible progress text, so values containing spaces can never
        // collide into the same signature and suppress a needed update.
        private static readonly string ProgressFieldSeparator = ((char)0x1f).ToString();

        private static string GetProgressSignature(ProgressRecord record) =>
            string.Join(ProgressFieldSeparator,
                record.ActivityId.ToString(CultureInfo.InvariantCulture),
                record.ParentActivityId.ToString(CultureInfo.InvariantCulture),
                record.Activity,
                record.StatusDescription,
                record.CurrentOperation,
                record.PercentComplete.ToString(CultureInfo.InvariantCulture),
                record.RecordType.ToString());
    }
}