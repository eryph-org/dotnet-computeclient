using System;
using System.Management.Automation;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.Operations
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Get, "EryphOperation", DefaultParameterSetName = "get")]
    [OutputType(typeof(Operation))]
    public class GetEryphOperationCommand : ComputeCmdLet
    {
        [Parameter(
            ParameterSetName = "get",
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string[] Id { get; set; }

        protected override void ProcessRecord()
        {
            if (Id != null)
            {
                foreach (var id in Id)
                {
                    WriteObject(Factory.CreateOperationsClient().Get(id).Value);
                }

                return;
            }

            ListOutput(Factory.CreateOperationsClient().List());


        }


    }
}