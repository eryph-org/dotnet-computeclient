using System;
using System.Management.Automation;
using Eryph.ClientRuntime.OData;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Get, "EryphMachine", DefaultParameterSetName = "get")]
    [OutputType(typeof(VirtualMachine))]
    public class GetEryphMachineCommand : ComputeCmdLet
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
                    WriteObject(GetSingleMachine(id));
                }

                return;
            }

            var query = new ODataQuery<Machine>();

            var page = ComputeClient.Machines.List();

            while (!Stopping)
            {
                WriteObject(page, true);

                if (string.IsNullOrWhiteSpace(page.NextPageLink))
                    break;

                page = ComputeClient.Machines.ListNext(page.NextPageLink);

            }

        }

    }

    }