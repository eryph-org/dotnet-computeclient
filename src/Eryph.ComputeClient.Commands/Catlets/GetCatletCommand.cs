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
    [Cmdlet(VerbsCommon.Get, "Catlet", DefaultParameterSetName = "get")]
    [OutputType(typeof(Catlet), ParameterSetName = new[] { "get" })]
    [OutputType(typeof(Catlet), ParameterSetName = new[] { "list" })]
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

        [Parameter(
            ParameterSetName = "list",
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty]
        public string ProjectName { get; set; }

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

            var projectId = GetProjectId(ProjectName);
            foreach (var virtualCatlet in Factory.CreateCatletsClient().List(projectId: projectId))
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

    public class ProjectNotFoundException : Exception
    {
        public ProjectNotFoundException()
        {
        }

        public ProjectNotFoundException(string project) : base($"Project {project} not found")
        {
        }

        public ProjectNotFoundException(string project, Exception inner) : base($"Project {project} not found", inner)
        {
        }
    }
}