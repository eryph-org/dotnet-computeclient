using System;
using System.Management.Automation;
using Eryph.ClientRuntime;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;
using Operation = Eryph.ComputeClient.Models.Operation;

namespace Eryph.ComputeClient.Commands.Catlets
{
    [PublicAPI]
    [Cmdlet(VerbsLifecycle.Start, "Catlet")]
    [OutputType(typeof(Operation), typeof(Catlet))]
    public class StartCatletCommand : CatletCmdLet
    {
        [Parameter(
            Position = 0,
            ValueFromPipeline = true,
            Mandatory = true,
            ValueFromPipelineByPropertyName = true)]
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

        [Parameter]
        public SwitchParameter Wait
        {
            get => _wait;
            set => _wait = value;
        }

        private bool _force;
        private bool _wait;
        private bool _yesToAll, _noToAll;


        protected override void ProcessRecord()
        {
            foreach (var id in Id)
            {
                Catlet catlet;
                try
                {
                    catlet = Factory.CreateCatletsClient().Get(id);
                }
                catch (Exception ex)
                {
                    WriteError(new ErrorRecord(ex, "CatletNotFound", ErrorCategory.ObjectNotFound, id));
                    continue;
                }

                if (!Force && !ShouldContinue($"Catlet '{catlet.Name}' (Id:{id}) will be started!", "Warning!",
                    ref _yesToAll, ref _noToAll))
                {
                    continue;
                }

                WaitForOperation(Factory.CreateCatletsClient().Start(id), _wait, false, id);

            }

        }

    }
}