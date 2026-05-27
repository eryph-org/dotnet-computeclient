using System;
using System.Management.Automation;
using Eryph.ComputeClient.Models;
using Eryph.ConfigModel.Catlets;
using Eryph.ConfigModel.Json;
using Eryph.ConfigModel.Yaml;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.Catlets
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Get, "Catlet", DefaultParameterSetName = "list")]
    [OutputType(typeof(Catlet), ParameterSetName = new[] { "get" })]
    [OutputType(typeof(Catlet), ParameterSetName = new[] { "list" })]
    [OutputType(typeof(string), ParameterSetName = new[] { "getconfig" })]
    public class GetCatletCommand : CatletCmdLet
    {
        [Parameter(
            ParameterSetName = "get",
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        // Positional only in the 'getconfig' set, so 'Get-Catlet <id> -Config' works.
        // The bare positional slot otherwise belongs to -Name in the (default) 'list' set,
        // consistent with the other Get-* cmdlets.
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

        [Parameter(
            ParameterSetName = "list",
            ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty]
        public string ProjectName { get; set; }

        [Parameter(
            ParameterSetName = "list",
            Position = 0)]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        protected override void ProcessRecord()
        {

            if (Id != null)
            {
                foreach (var id in Id)
                {
                    if (Stopping) break;

                    if (Config.IsPresent)
                    {
                        if (TryGetById(id, i => Factory.CreateCatletsClient().GetConfig(i), "catlet", out var config))
                            WriteConfig(config);
                    }
                    else
                    {
                        if (TryGetById(id, GetSingleCatlet, "catlet", out var catlet))
                            WriteObject(catlet);
                    }
                }

                return;
            }

            if (!TryGetProjectId(ProjectName, out var projectId))
                return;

            // 'Get-Catlet -Config' (without -Id) dumps the config of every catlet in
            // scope. -Config lives in the 'getconfig' set, so -Name is never set here.
            if (Config.IsPresent)
            {
                var catletsClient = Factory.CreateCatletsClient();
                foreach (var catlet in catletsClient.List(projectId: projectId))
                {
                    if (Stopping) break;
                    WriteConfig(catletsClient.GetConfig(catlet.Id));
                }

                return;
            }

            WriteByNameOrId(
                Name,
                GetSingleCatlet,
                () => Factory.CreateCatletsClient().List(projectId: projectId),
                catlet => catlet.Name,
                "catlet");
        }

        private void WriteConfig(CatletConfiguration config)
        {
            var catletConfig = CatletConfigJsonSerializer.Deserialize(
                config.Configuration);

            var yaml = CatletConfigYamlSerializer.Serialize(catletConfig);
            WriteObject(yaml);

        }
    }
}