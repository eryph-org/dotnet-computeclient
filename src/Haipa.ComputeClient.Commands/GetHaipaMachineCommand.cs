using System;
using System.Management.Automation;
using Haipa.ClientRuntime.OData;
using Haipa.ComputeClient.Models;
using JetBrains.Annotations;

namespace Haipa.ComputeClient.Commands
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Get,"HaipaMachine", DefaultParameterSetName = "get")]
    [OutputType(typeof(VirtualMachine))]
    public class GetHaipaMachineCommand : ComputeCmdLet
    {
        [Parameter(
            ParameterSetName = "get",
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public Guid[] Id { get; set; }

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

            var page = ComputeClient.Machines.List(query);

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