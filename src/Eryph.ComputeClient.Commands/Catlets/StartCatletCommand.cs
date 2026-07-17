using System;
using System.Management.Automation;
using Eryph.ClientRuntime;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;
using Operation = Eryph.ComputeClient.Models.Operation;

namespace Eryph.ComputeClient.Commands.Catlets
{
    [PublicAPI]
    [Cmdlet(VerbsLifecycle.Start, "Catlet", DefaultParameterSetName = "Wait")]
    [OutputType(typeof(Catlet), ParameterSetName = new[] { "Wait" })]
    [OutputType(typeof(Operation), ParameterSetName = new[] { "NoWait" })]
    public class StartCatletCommand : CatletCmdLet
    {
        [Parameter(
            Position = 0,
            ValueFromPipeline = true,
            Mandatory = true,
            ValueFromPipelineByPropertyName = true)]
        [Alias("Name")]
        public string[] Id { get; set; }

        /// <summary>
        /// This parameter overrides the ShouldContinue call to force
        /// the cmdlet to continue its operation. This parameter should always
        /// be used with caution.
        /// </summary>
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

        [Parameter]
        [ValidateNotNullOrEmpty]
        public string ProjectName { get; set; }

        [Parameter]
        [ValidateNotNullOrEmpty]
        public string Environment { get; set; }

        private bool _force;
        private bool _nowait;
        private bool _yesToAll, _noToAll;


        protected override void ProcessRecord()
        {
            foreach (var nameOrId in Id)
            {
                foreach (var catlet in ResolveActionTargets(nameOrId, ProjectName, GetSingleCatlet,
                             projectId => ListCatlets(projectId, Environment), c => c.Name, CatletResourceKind, EnvironmentHintIfUnset(Environment)))
                {
                    if (Stopping) break;

                    if (!Force && !ShouldContinue($"Catlet '{catlet.Name}' (Id:{catlet.Id}) will be started!", "Warning!",
                        ref _yesToAll, ref _noToAll))
                    {
                        continue;
                    }

                    WaitForOperation(Factory.CreateCatletsClient().Start(catlet.Id), _nowait, false, catlet.Id);
                }
            }
        }

    }
}