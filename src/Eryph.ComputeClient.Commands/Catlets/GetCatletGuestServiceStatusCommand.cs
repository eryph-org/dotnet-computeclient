using System.Management.Automation;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.Catlets
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Get, "CatletGuestServiceStatus", DefaultParameterSetName = "list")]
    [OutputType(typeof(CatletGuestServiceStatus))]
    public class GetCatletGuestServiceStatusCommand : CatletGuestServiceGetCmdlet
    {
        // Reports only the runtime state reported by the guest agent. The configured
        // shell is configuration, not status, and is surfaced by Get-CatletGuestServiceConfig.
        protected override void WriteResult(Catlet catlet, GuestServicesStatusOperationResult result)
        {
            WriteObject(new CatletGuestServiceStatus
            {
                Id = catlet.Id,
                Name = catlet.Name,
                Project = catlet.Project?.Name,
                GuestServicesStatus = result.GuestServicesStatus,
                GuestServicesVersion = result.GuestServicesVersion,
                ProvisioningState = result.ProvisioningState,
            });
        }
    }

    [PublicAPI]
    public class CatletGuestServiceStatus
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Project { get; set; }

        public string GuestServicesStatus { get; set; }

        public string GuestServicesVersion { get; set; }

        public string ProvisioningState { get; set; }
    }
}
