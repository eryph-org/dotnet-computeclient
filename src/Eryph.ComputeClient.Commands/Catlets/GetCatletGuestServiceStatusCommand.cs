using System.Management.Automation;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.Catlets
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Get, "CatletGuestServiceStatus", DefaultParameterSetName = "list")]
    [OutputType(typeof(PSObject))]
    public class GetCatletGuestServiceStatusCommand : CatletGuestServiceGetCmdlet
    {
        // Reports only the runtime state reported by the guest agent. The configured
        // shell is configuration, not status, and is surfaced by Get-CatletGuestServiceConfig.
        protected override void WriteResult(Catlet catlet, GuestServicesStatusOperationResult result)
        {
            var output = new PSObject();
            output.TypeNames.Insert(0, "Eryph.ComputeClient.GuestServiceStatus");
            output.Properties.Add(new PSNoteProperty("CatletId", catlet.Id));
            output.Properties.Add(new PSNoteProperty("CatletName", catlet.Name));
            output.Properties.Add(new PSNoteProperty("GuestServicesStatus", result.GuestServicesStatus));
            output.Properties.Add(new PSNoteProperty("GuestServicesVersion", result.GuestServicesVersion));
            output.Properties.Add(new PSNoteProperty("ProvisioningState", result.ProvisioningState));
            WriteObject(output);
        }
    }
}
