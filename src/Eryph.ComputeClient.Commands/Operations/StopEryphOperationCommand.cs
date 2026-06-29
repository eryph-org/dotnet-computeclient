using System;
using System.Management.Automation;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.Operations
{
    [PublicAPI]
    [Cmdlet(VerbsLifecycle.Stop, "EryphOperation")]
    [OutputType(typeof(Operation))]
    public class StopEryphOperationCommand : ComputeCmdLet
    {
        [Parameter(
            Position = 0,
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string[] Id { get; set; }

        /// <summary>
        /// This parameter overrides the ShouldContinue call to force
        /// the cmdlet to cancel the operation. This parameter should always
        /// be used with caution.
        /// </summary>
        [Parameter]
        public SwitchParameter Force { get; set; }

        [Parameter]
        public SwitchParameter PassThru { get; set; }

        private bool _yesToAll;
        private bool _noToAll;

        protected override void ProcessRecord()
        {
            if (Id is null)
                return;

            foreach (var id in Id)
            {
                if (Stopping) break;

                if (!Force && !ShouldContinue(
                        $"Operation '{id}' will be cancelled. Cancellation is best-effort: "
                        + "only tasks whose handlers support cancellation are interrupted.",
                        "Warning!",
                        ref _yesToAll, ref _noToAll))
                {
                    continue;
                }

                var operationsClient = Factory.CreateOperationsClient();
                try
                {
                    operationsClient.Cancel(id);
                }
                catch (Exception ex)
                {
                    WriteError(new ErrorRecord(ex, "CancelOperationFailed", ErrorCategory.InvalidOperation, id));
                    continue;
                }

                if (PassThru)
                    WriteObject(operationsClient.Get(id).Value);
            }
        }
    }
}
