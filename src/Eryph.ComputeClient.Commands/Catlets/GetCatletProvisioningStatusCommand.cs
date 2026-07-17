using System.Management.Automation;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.Catlets
{
    /// <summary>
    /// Reports the catlet's provisioning status. Unlike the guest-services state,
    /// the provisioning status is carried directly on the catlet resource, so it is
    /// read straight from <c>Get</c>/<c>List</c> without starting an operation.
    /// The provisioning status (<see cref="CatletProvisioningStatus"/>) reflects the
    /// catlet-level provisioning progress and is distinct from the guest agent's own
    /// provisioning state surfaced by Get-CatletGuestServiceStatus.
    /// </summary>
    [PublicAPI]
    [Cmdlet(VerbsCommon.Get, "CatletProvisioningStatus", DefaultParameterSetName = "list")]
    [OutputType(typeof(CatletProvisioningStatusInfo))]
    public class GetCatletProvisioningStatusCommand : CatletCmdLet
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
                        WriteStatus(catlet);
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
                WriteStatus);
        }

        private void WriteStatus(Catlet catlet)
        {
            WriteObject(new CatletProvisioningStatusInfo
            {
                Id = catlet.Id,
                Name = catlet.Name,
                Project = catlet.Project?.Name,
                Status = catlet.Status.ToString(),
                ProvisioningStatus = catlet.ProvisioningStatus.ToString(),
            });
        }
    }

    [PublicAPI]
    public class CatletProvisioningStatusInfo
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Project { get; set; }

        public string Status { get; set; }

        public string ProvisioningStatus { get; set; }
    }
}
