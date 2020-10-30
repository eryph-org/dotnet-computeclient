using System;
using System.Management.Automation;
using Haipa.ClientRuntime.OData;
using Haipa.ComputeClient.Models;
using JetBrains.Annotations;

namespace Haipa.ComputeClient.Commands
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Get,"HaipaVM", DefaultParameterSetName = "get")]
    [OutputType(typeof(VirtualMachine))]
    public class GetHaipaVMCommand : ComputeCmdLet
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
                    WriteObject(GetSingleVM(id));
                }

                return;
            }

            var query = new ODataQuery<VirtualMachine>();

            var page = ComputeClient.VirtualMachines.List(query);

            while (!Stopping)
            {
                WriteObject(page, true);

                if (string.IsNullOrWhiteSpace(page.NextPageLink))
                    break;

                page = ComputeClient.VirtualMachines.ListNext(page.NextPageLink);

            }

        }

    }

    }