using System;
using System.Management.Automation;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;
using Operation = Eryph.ComputeClient.Models.Operation;

namespace Eryph.ComputeClient.Commands.Catlets
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Remove, "Catlet", DefaultParameterSetName = "Wait")]
    [OutputType(typeof(Catlet), ParameterSetName = new[] { "Wait" })]
    [OutputType(typeof(Operation), ParameterSetName = new[] { "NoWait" })]
    public class RemoveCatletCommand : CatletCmdLet
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
        /// the cmdlet to stop its operation. This parameter should always
        /// be used with caution.
        /// </summary>
        [Parameter]
        public SwitchParameter Force
        {
            get => _force;
            set => _force = value;
        }

        /// <summary>
        /// This parameter indicates that the cmdlet should return
        /// an object to the pipeline after the processing has been
        /// completed.
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

        [Parameter]
        [ValidateNotNullOrEmpty]
        public string ProjectName { get; set; }

        private bool _force;
        private bool _nowait;
        private bool _passThru;

        private bool _yesToAll, _noToAll;



        protected override void ProcessRecord()
        {
            foreach (var nameOrId in Id)
            {
                foreach (var catlet in ResolveActionTargets(nameOrId, ProjectName, GetSingleCatlet,
                             projectId => Factory.CreateCatletsClient().List(projectId: projectId), c => c.Name, "catlet"))
                {
                    if (Stopping) break;

                    if (!Force && !ShouldContinue($"Catlet '{catlet.Name}' (Id:{catlet.Id}) will be deleted!", "Warning!",
                        ref _yesToAll, ref _noToAll))
                    {
                        continue;
                    }

                    WaitForOperation(Factory.CreateCatletsClient().Delete(catlet.Id).Value, _nowait, false, catlet.Id);

                    if (PassThru)
                        WriteObject(catlet);
                }
            }

        }

    }
}