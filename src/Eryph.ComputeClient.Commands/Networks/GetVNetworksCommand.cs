using System.Management.Automation;
using Eryph.ComputeClient.Models;
using Eryph.ConfigModel.Catlets;
using Eryph.ConfigModel.Json;
using Eryph.ConfigModel.Networks;
using Eryph.ConfigModel.Yaml;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.Networks
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Get, "VNetwork", DefaultParameterSetName = "get")]
    [OutputType(typeof(VirtualNetwork), ParameterSetName = new[] { "get" })]
    [OutputType(typeof(string), ParameterSetName = new[] { "getconfig" })]
    public class GetVNetworksCommand : NetworkCmdLet
    {
        [Parameter(
            ParameterSetName = "get",
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]

        public string[] Id { get; set; }

        [Parameter(
            ParameterSetName = "getconfig",
            Mandatory = true)]
        public SwitchParameter Config { get; set; }

        [Parameter(
            ParameterSetName = "getconfig",
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string ProjectName { get; set; }

        protected override void ProcessRecord()
        {
            if (Config.IsPresent)
            {
                WriteConfig(Factory.CreateVNetworksClient().GetConfig(ProjectName).Value);
                return;
            }


            if (Id != null)
            {
                foreach (var id in Id)
                {
                    WriteObject(GetSingleNetwork(id));
                }

                return;
            }


            foreach (var virtualCatlet in Factory.CreateVNetworksClient().List())
            {
                if (Stopping) break;

                if (Config.IsPresent)
                {
                    WriteConfig(Factory.CreateVNetworksClient().GetConfig(virtualCatlet.Id));

                }
                else
                {
                    WriteObject(virtualCatlet, true);
                }
            }


        }

        private void WriteConfig(VirtualNetworkConfiguration config)
        {

            var catletConfig = ProjectNetworksConfigDictionaryConverter.Convert(
                ConfigModelJsonSerializer.DeserializeToDictionary(config.Configuration));

            var yaml = ProjectNetworkConfigYamlSerializer.Serialize(catletConfig);
            WriteObject(yaml);

        }

    }

}