using System.Management.Automation;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.Catlets
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Get, "CatletDisk", DefaultParameterSetName = "get")]
    [OutputType(typeof(VirtualDisk))]
    public class GetCatletDiskCommand : ComputeCmdLet
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
                    WriteObject(Factory.CreateVirtualDisksClient().Get(id));
                }

                return;
            }

            ListOutput(Factory.CreateVirtualDisksClient().List());


        }

    }

}