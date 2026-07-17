using System;
using System.Management.Automation;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.Catlets
{
    /// <summary>
    /// Reads the catlet's provisioning log, reassembled from the guest's cloud-init
    /// telemetry. The log is not returned directly: <c>GetProvisioningLog</c> starts an
    /// operation whose result carries both a rendered, human-readable text log and the
    /// structured events. By default the structured events are written to the pipeline
    /// as <see cref="CatletProvisioningLogEntry"/> objects (each carrying the catlet it
    /// belongs to, so output stays correlatable when several catlets are targeted);
    /// <c>-AsText</c> emits the rendered text log instead, prefixed with a catlet header.
    /// </summary>
    [PublicAPI]
    [Cmdlet(VerbsCommon.Get, "CatletProvisioningLog", DefaultParameterSetName = "list")]
    [OutputType(typeof(CatletProvisioningLogEntry))]
    [OutputType(typeof(string))]
    public class GetCatletProvisioningLogCommand : CatletCmdLet
    {
        [Parameter(
            ParameterSetName = "get",
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string[] Id { get; set; }

        [Parameter(
            ParameterSetName = "list",
            Position = 0)]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        [Parameter(
            ParameterSetName = "list",
            ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty]
        public string ProjectName { get; set; }

        /// <summary>
        /// Emit the rendered, human-readable text log instead of the structured events.
        /// </summary>
        [Parameter]
        public SwitchParameter AsText { get; set; }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            RequireApiVersion(1, 2, MyInvocation.MyCommand.Name);
        }

        protected override void ProcessRecord()
        {
            if (Id != null)
            {
                foreach (var id in Id)
                {
                    if (Stopping) break;
                    if (TryGetById(id, GetSingleCatlet, "catlet", out var catlet))
                        EmitLog(catlet);
                }

                return;
            }

            if (!TryGetProjectId(ProjectName, out var projectId))
                return;

            WriteByNameOrId(
                Name,
                GetSingleCatlet,
                () => Factory.CreateCatletsClient().List(projectId: projectId),
                catlet => catlet.Name,
                "catlet",
                EmitLog);
        }

        private void EmitLog(Catlet catlet)
        {
            var operation = WaitForOperation(
                Factory.CreateCatletsClient().GetProvisioningLog(catlet.Id));

            if (operation.Status != OperationStatus.Completed)
                return;

            if (operation.Result is not ProvisioningLogOperationResult result)
                return;

            if (AsText.IsPresent)
            {
                // Prefix a header so the text blocks stay attributable to their catlet
                // when several catlets are targeted in one invocation.
                var header = $"# Catlet {catlet.Name} ({catlet.Id})";
                WriteObject(header + Environment.NewLine + result.RenderedLog);
                return;
            }

            foreach (var entry in result.Events)
            {
                if (Stopping) break;
                WriteObject(new CatletProvisioningLogEntry
                {
                    CatletId = catlet.Id,
                    CatletName = catlet.Name,
                    Project = catlet.Project?.Name,
                    Timestamp = entry.Timestamp,
                    Type = entry.Type,
                    Name = entry.Name,
                    Result = entry.Result,
                    Message = entry.Message,
                });
            }
        }
    }

    /// <summary>
    /// A single provisioning-log event, tagged with the catlet it belongs to so the
    /// pipeline output can be correlated back to a catlet when several are targeted.
    /// </summary>
    [PublicAPI]
    public class CatletProvisioningLogEntry
    {
        public string CatletId { get; set; }

        public string CatletName { get; set; }

        public string Project { get; set; }

        public DateTimeOffset? Timestamp { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }

        public string Result { get; set; }

        public string Message { get; set; }
    }
}
