using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using Azure;
using Eryph.ClientRuntime.Configuration;
using Eryph.ClientRuntime.Powershell;
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
                    IsLoggingContentEnabled = IsDebugEnabled
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


        protected void ResourceWriter(Operation operation, Action<ResourceType, string> resourceWriterDelegate)
        {
            var resourceData =
                Factory.CreateOperationsClient().Get(operation.Id, expand: "resources").Value;

            try
            {

                foreach (var resource in resourceData.Resources.Where(x => !string.IsNullOrWhiteSpace(x.ResourceId)))
                {
                    resourceWriterDelegate(resource.ResourceType.GetValueOrDefault(), resource.ResourceId);
                }

                if (resourceData.Resources.Count > 0)
                    return;
            }
            catch
            {
                WriteObject(resourceData);
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

        protected void WaitForOperation(Operation operation, Action<Operation> writerDelegate = null)
        {
            var timeStamp = DateTime.Parse("2018-01-01", CultureInfo.InvariantCulture);


            var processedLogIds = new List<string>();
            while (!Stopping)
            {
                Task.Delay(1000).GetAwaiter().GetResult();

                var currentOperation = Factory.CreateOperationsClient()
                    .Get(operation.Id, timeStamp, expand: "logs").Value;

                foreach (var logEntry in currentOperation.LogEntries)
                {
                    if (processedLogIds.Contains(logEntry.Id))
                        continue;

                    processedLogIds.Add(logEntry.Id);

                    WriteVerbose($"Operation {currentOperation.Id}: {logEntry.Message}");
                    timeStamp = logEntry.Timestamp.GetValueOrDefault().DateTime;
                    Task.Delay(100).GetAwaiter().GetResult();
                }

                switch (currentOperation.Status.GetValueOrDefault().ToString())
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


            if (writerDelegate != null)
            {
                writerDelegate(operation);
                return;
            }

            WriteObject(Factory.CreateOperationsClient().Get(operation.Id).Value);
        }
    }
}