using System.Management.Automation;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.Catlets
{
    /// <summary>
    /// Shared base for the read cmdlets that project the guest-services state of a
    /// catlet. The state is not returned directly: <c>GetGuestServices</c> starts an
    /// operation whose result carries the agent status, version, provisioning state
    /// and the configured shell. This base resolves the target catlet(s), runs the
    /// operation and hands the typed result to the derived cmdlet, which decides which
    /// part of it to surface (runtime status vs. configuration).
    /// </summary>
    [PublicAPI]
    public abstract class CatletGuestServiceGetCmdlet : CatletCmdLet
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
                        EmitCatlet(catlet);
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
                EmitCatlet);
        }

        private void EmitCatlet(Catlet catlet)
        {
            var operation = WaitForOperation(
                Factory.CreateCatletsClient().GetGuestServices(catlet.Id));

            if (operation.Status != OperationStatus.Completed)
                return;

            if (operation.Result is not GuestServicesStatusOperationResult result)
                return;

            WriteResult(catlet, result);
        }

        /// <summary>
        /// Projects the part of the guest-services result this cmdlet is responsible for.
        /// </summary>
        protected abstract void WriteResult(Catlet catlet, GuestServicesStatusOperationResult result);
    }
}
