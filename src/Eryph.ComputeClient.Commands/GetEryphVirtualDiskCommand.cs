using System;
using System.Management.Automation;
using Eryph.ClientRuntime.OData;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Get, "EryphVirtualDisk", DefaultParameterSetName = "get")]
    [OutputType(typeof(VirtualDisk))]
    public class GetEryphVirtualDiskCommand : ComputeCmdLet
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
                    WriteObject(ComputeClient.VirtualDisks.Get(id));
                }

                return;
            }

            var page = ComputeClient.VirtualDisks.List();

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