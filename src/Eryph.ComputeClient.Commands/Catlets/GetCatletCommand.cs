using System.Management.Automation;
using Eryph.ComputeClient.Models;
using Eryph.ConfigModel.Catlets;
using Eryph.ConfigModel.Json;
using Eryph.ConfigModel.Yaml;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.Catlets
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Get, "Catlet", DefaultParameterSetName = "get")]
    [OutputType(typeof(Catlet), ParameterSetName = new[] { "get" })]
    [OutputType(typeof(string), ParameterSetName = new[] { "getconfig" })]
    public class GetCatletCommand : CatletCmdLet
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
                        WriteConfig(Factory.CreateCatletsClient().GetConfig(id));
                    else
                        WriteObject(GetSingleCatlet(id));
                }

                return;
            }


            foreach (var virtualCatlet in Factory.CreateCatletsClient().List())
            {
                if (Stopping) break;

                if (Config.IsPresent)
                {
                    WriteConfig(Factory.CreateCatletsClient().GetConfig(virtualCatlet.Id));

                }
                else
                {
                    WriteObject(virtualCatlet, true);
                }
            }


        }

        private void WriteConfig(CatletConfiguration config)
        {
            var catletConfig = CatletConfigDictionaryConverter.Convert(
                ConfigModelJsonSerializer.DeserializeToDictionary(config.Configuration));

            var yaml = CatletConfigYamlSerializer.Serialize(catletConfig);
            WriteObject(yaml);

        }

    }

}