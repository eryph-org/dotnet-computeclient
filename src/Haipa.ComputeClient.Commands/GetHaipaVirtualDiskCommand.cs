using System;
using System.Management.Automation;
using Haipa.ClientRuntime.OData;
using Haipa.ComputeClient.Models;
using JetBrains.Annotations;

namespace Haipa.ComputeClient.Commands
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Get,"HaipaVirtualDisk", DefaultParameterSetName = "get")]
    [OutputType(typeof(VirtualDisk))]
    public class GetHaipaVirtualDiskCommand : ComputeCmdLet
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
                    WriteObject(ComputeClient.VirtualDisks.Get(id));
                }

                return;
            }

            var query = new ODataQuery<VirtualDisk>();

            var page = ComputeClient.VirtualDisks.List(query);

            while (!Stopping)
            {
                WriteObject(page, true);

                if (string.IsNullOrWhiteSpace(page.NextPageLink))
                    break;

                page = ComputeClient.VirtualDisks.ListNext(page.NextPageLink);

            }

        }

    }

    }