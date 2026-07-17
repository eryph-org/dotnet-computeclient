using System.Management.Automation;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.Catlets
{
    /// <summary>
    /// Reads the catlet's provisioning log, reassembled from the guest's cloud-init
    /// telemetry. The log is not returned directly: <c>GetProvisioningLog</c> starts an
    /// operation whose result carries both a rendered, human-readable text log and the
    /// structured events. By default the structured <see cref="ProvisioningLogEntry"/>
    /// events are written to the pipeline; <c>-AsText</c> emits the text log instead.
    /// </summary>
    [PublicAPI]
    [Cmdlet(VerbsCommon.Get, "CatletProvisioningLog", DefaultParameterSetName = "list")]
    [OutputType(typeof(ProvisioningLogEntry))]
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
                WriteObject(result.RenderedLog);
                return;
            }

            foreach (var entry in result.Events)
            {
                if (Stopping) break;
                WriteObject(entry);
            }
        }
    }
}
