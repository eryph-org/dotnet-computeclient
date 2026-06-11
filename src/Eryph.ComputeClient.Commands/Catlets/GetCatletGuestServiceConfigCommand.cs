using System.Management.Automation;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.Catlets
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Get, "CatletGuestServiceConfig", DefaultParameterSetName = "list")]
    [OutputType(typeof(PSObject))]
    public class GetCatletGuestServiceConfigCommand : CatletGuestServiceGetCmdlet
    {
        // Reports the guest-services configuration. Only the shell is readable: the
        // GetGuestServices result echoes the configured Shell override but not ShellArgs,
        // so ShellArgs can be set (Set-CatletGuestServiceConfig) but not read back here.
        protected override void WriteResult(Catlet catlet, GuestServicesStatusOperationResult result)
        {
            var output = new PSObject();
            output.TypeNames.Insert(0, "Eryph.ComputeClient.GuestServiceConfig");
            output.Properties.Add(new PSNoteProperty("CatletId", catlet.Id));
            output.Properties.Add(new PSNoteProperty("CatletName", catlet.Name));
            output.Properties.Add(new PSNoteProperty("Shell", result.Shell));
            WriteObject(output);
        }
    }
}
