using System.Management.Automation;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;
using Operation = Eryph.ComputeClient.Models.Operation;

namespace Eryph.ComputeClient.Commands.Catlets
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Set, "CatletGuestServiceConfig", DefaultParameterSetName = "Wait")]
    [OutputType(typeof(Catlet), ParameterSetName = new[] { "Wait" })]
    [OutputType(typeof(Operation), ParameterSetName = new[] { "NoWait" })]
    public class SetCatletGuestServiceConfigCommand : CatletCmdLet
    {
        [Parameter(
            Position = 0,
            ValueFromPipeline = true,
            Mandatory = true,
            ValueFromPipelineByPropertyName = true)]
        [Alias("Name")]
        public string[] Id { get; set; }

        [Parameter]
        [ValidateNotNullOrEmpty]
        public string ProjectName { get; set; }

        /// <summary>
        /// Shell command for interactive SSH sessions. An empty string clears the
        /// override; omitting the parameter leaves the current value unchanged.
        /// </summary>
        [Parameter]
        [AllowEmptyString]
        public string Shell { get; set; }

        /// <summary>
        /// Arguments for the shell command. Same empty/omitted semantics as <see cref="Shell"/>.
        /// </summary>
        [Parameter]
        [AllowEmptyString]
        public string ShellArgs { get; set; }

        [Parameter]
        public SwitchParameter Force
        {
            get => _force;
            set => _force = value;
        }

        [Parameter(ParameterSetName = "NoWait")]
        public SwitchParameter NoWait
        {
            get => _nowait;
            set => _nowait = value;
        }

        private bool _force;
        private bool _nowait;
        private bool _yesToAll, _noToAll;

        private bool _shellBound;
        private bool _shellArgsBound;

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            RequireApiVersion(1, 2, "Set-CatletGuestServiceConfig");

            _shellBound = MyInvocation.BoundParameters.ContainsKey(nameof(Shell));
            _shellArgsBound = MyInvocation.BoundParameters.ContainsKey(nameof(ShellArgs));

            if (!_shellBound && !_shellArgsBound)
            {
                ThrowTerminatingError(new ErrorRecord(
                    new PSArgumentException(
                        "Specify at least one setting to change: -Shell and/or -ShellArgs. "
                        + "Pass an empty string to clear a setting."),
                    "NoGuestServiceSettingSpecified",
                    ErrorCategory.InvalidArgument,
                    null));
            }
        }

        protected override void ProcessRecord()
        {
            foreach (var nameOrId in Id)
            {
                foreach (var catlet in ResolveActionTargets(nameOrId, ProjectName, GetSingleCatlet,
                             projectId => Factory.CreateCatletsClient().List(projectId: projectId), c => c.Name, "catlet"))
                {
                    if (Stopping) break;

                    if (!Force && !ShouldContinue(
                            $"The guest-services configuration of catlet '{catlet.Name}' (Id:{catlet.Id}) will be updated!",
                            "Warning!", ref _yesToAll, ref _noToAll))
                    {
                        continue;
                    }

                    // Only the settings the caller specified are sent. An unset property
                    // stays null and is omitted from the request, so the server leaves it
                    // unchanged; an empty string is sent and clears the override.
                    var body = new GuestServicesSettingsBody();
                    if (_shellBound)
                        body.Shell = Shell;
                    if (_shellArgsBound)
                        body.ShellArgs = ShellArgs;

                    WaitForOperation(
                        Factory.CreateCatletsClient().SetGuestServicesSettings(catlet.Id, body),
                        _nowait, false, catlet.Id);
                }
            }
        }
    }
}
