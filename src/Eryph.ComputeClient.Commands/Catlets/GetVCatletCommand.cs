using System.Management.Automation;
using Eryph.ComputeClient.Models;
using Eryph.ConfigModel.Catlets;
using Eryph.ConfigModel.Json;
using Eryph.ConfigModel.Yaml;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Eryph.ComputeClient.Commands.Catlets
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Get, "VCatlet", DefaultParameterSetName = "get")]
    [OutputType(typeof(VirtualCatlet), ParameterSetName = new[] { "get" })]
    [OutputType(typeof(string), ParameterSetName = new[] { "getconfig" })]
    public class GetVCatletCommand : CatletCmdLet
    {
        [Parameter(
            ParameterSetName = "get",
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        [Parameter(
            ParameterSetName = "getconfig",
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string[] Id { get; set; }

        [Parameter(
            ParameterSetName = "getconfig",
            Mandatory = true)]
        public SwitchParameter Config { get; set; }

        protected override void ProcessRecord()
        {

            if (Id != null)
            {
                foreach (var id in Id)
                {
                    if (Config.IsPresent)
                        WriteConfig(Factory.CreateVCatletsClient().GetConfig(id));
                    else
                        WriteObject(GetSingleVM(id));
                }

                return;
            }


            foreach (var virtualCatlet in Factory.CreateVCatletsClient().List())
            {
                if (Stopping) break;

                if (Config.IsPresent)
                {
                    WriteConfig(Factory.CreateVCatletsClient().GetConfig(virtualCatlet.Id));

                }
                else
                {
                    WriteObject(virtualCatlet, true);
                }
            }


        }

        private void WriteConfig(VirtualCatletConfiguration config)
        {

            var catletConfig = CatletConfigDictionaryConverter.Convert(
                ConfigModelJsonSerializer.DeserializeToDictionary(config.Configuration));

            var yaml = CatletConfigYamlSerializer.Serialize(catletConfig);
            WriteObject(yaml);

        }

    }

}