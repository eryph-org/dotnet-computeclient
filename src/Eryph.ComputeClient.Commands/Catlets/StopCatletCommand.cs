using System;
using System.Management.Automation;
using Eryph.ClientRuntime;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;


namespace Eryph.ComputeClient.Commands.Catlets
{
    [PublicAPI]
    [Cmdlet(VerbsLifecycle.Stop, "Catlet")]
    [OutputType(typeof(Operation), typeof(Catlet))]
    public class StopCatletCommand : CatletCmdLet
    {
        [Parameter(
            Position = 0,
            ValueFromPipeline = true,
            Mandatory = true,
            ValueFromPipelineByPropertyName = true)]
        public string[] Id { get; set; }

        /// <summary>
        /// This parameter overrides the ShouldContinue call to force
        /// the cmdlet to stop its operation. This parameter should always
        /// be used with caution.
        /// </summary>
        [Parameter]
        public SwitchParameter Force { get; set; }

        [Parameter]
        public SwitchParameter NoWait { get; set; }

        [Parameter]
        public CatletStopMode Mode { get; set; }

        private bool _yesToAll, _noToAll;

        protected override void ProcessRecord()
        {
            var stopMode = Mode switch
            {
                CatletStopMode.Shutdown => Models.CatletStopMode.Shutdown,
                CatletStopMode.Hard => Models.CatletStopMode.Hard,
                _ => throw new ArgumentOutOfRangeException(nameof(Mode))
            };

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

                if (!Force && !ShouldContinue($"Catlet '{catlet.Name}' (Id:{id}) will be stopped!", "Warning!",
                    ref _yesToAll, ref _noToAll))
                {
                    continue;
                }

                WaitForOperation(
                    Factory.CreateCatletsClient().Stop(id, new StopCatletRequestBody(stopMode)).Value,
                    NoWait, false, id);
            }
        }
    }
}
