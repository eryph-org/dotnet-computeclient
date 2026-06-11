using System.Management.Automation;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.Catlets
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Get, "CatletGuestServiceConfig", DefaultParameterSetName = "list")]
    [OutputType(typeof(CatletGuestServiceConfig))]
    public class GetCatletGuestServiceConfigCommand : CatletGuestServiceGetCmdlet
    {
        // Reports the guest-services configuration. Only the shell is readable: the
        // GetGuestServices result echoes the configured Shell override but not ShellArgs,
        // so ShellArgs can be set (Set-CatletGuestServiceConfig) but not read back here.
        protected override void WriteResult(Catlet catlet, GuestServicesStatusOperationResult result)
        {
            WriteObject(new CatletGuestServiceConfig
            {
                Id = catlet.Id,
                Name = catlet.Name,
                Project = catlet.Project?.Name,
                Shell = result.Shell,
            });
        }
    }

    [PublicAPI]
    public class CatletGuestServiceConfig
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Project { get; set; }

        public string Shell { get; set; }
    }
}
