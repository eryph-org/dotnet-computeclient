using System;
using System.Management.Automation;
using Eryph.ClientRuntime.OData;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Get, "EryphVM", DefaultParameterSetName = "get")]
    [OutputType(typeof(VirtualMachine))]
    public class GetEryphVMCommand : ComputeCmdLet
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
                    WriteObject(GetSingleVM(id));
                }

                return;
            }


            var page = ComputeClient.VirtualMachines.List();

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