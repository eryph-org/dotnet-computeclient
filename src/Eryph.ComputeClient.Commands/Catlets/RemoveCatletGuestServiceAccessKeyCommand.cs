using System.Management.Automation;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;
using Operation = Eryph.ComputeClient.Models.Operation;

namespace Eryph.ComputeClient.Commands.Catlets
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Remove, "CatletGuestServiceAccessKey", DefaultParameterSetName = "Wait")]
    [OutputType(typeof(Catlet), ParameterSetName = new[] { "Wait" })]
    [OutputType(typeof(Operation), ParameterSetName = new[] { "NoWait" })]
    public class RemoveCatletGuestServiceAccessKeyCommand : CatletCmdLet
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

        [Parameter]
        [ValidateNotNullOrEmpty]
        public string Environment { get; set; }

        [Parameter]
        public SwitchParameter Force
        {
            get => _force;
            set => _force = value;
        }

        /// <summary>
        /// Returns the catlet whose access key was revoked. By default this cmdlet
        /// produces no output.
        /// </summary>
        [Parameter]
        public SwitchParameter PassThru
        {
            get => _passThru;
            set => _passThru = value;
        }

        [Parameter(ParameterSetName = "NoWait")]
        public SwitchParameter NoWait
        {
            get => _nowait;
            set => _nowait = value;
        }

        private bool _force;
        private bool _nowait;
        private bool _passThru;
        private bool _yesToAll, _noToAll;

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            RequireApiVersion(1, 2, "Remove-CatletGuestServiceAccessKey");
        }

        protected override void ProcessRecord()
        {
            foreach (var nameOrId in Id)
            {
                foreach (var catlet in ResolveActionTargets(nameOrId, ProjectName, GetSingleCatlet,
                             projectId => ListCatlets(projectId, Environment), c => c.Name, CatletResourceKind, EnvironmentHintIfUnset(Environment)))
                {
                    if (Stopping) break;

                    if (!Force && !ShouldContinue(
                            $"The caller's guest-services access key for catlet '{catlet.Name}' (Id:{catlet.Id}) will be revoked!",
                            "Warning!", ref _yesToAll, ref _noToAll))
                    {
                        continue;
                    }

                    // Revokes only the caller's own authorized SSH key; the catlet itself
                    // is unchanged, so we do not emit it unless -PassThru is requested.
                    Operation operation = Factory.CreateCatletsClient().RemoveAccessKey(catlet.Id);

                    if (_nowait)
                        WriteObject(operation);
                    else
                        WaitForOperation(operation);

                    if (_passThru)
                        WriteObject(catlet);
                }
            }
        }
    }
}
