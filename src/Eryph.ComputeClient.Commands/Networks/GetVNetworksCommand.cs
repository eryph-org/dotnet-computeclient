﻿using System.Management.Automation;
using Eryph.ComputeClient.Models;
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
            ParameterSetName = "get",
            ValueFromPipelineByPropertyName = true)]
        [Parameter(
            ParameterSetName = "getconfig",
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string ProjectName { get; set; }

        protected override void ProcessRecord()
        {
            var projectId = GetProjectId(ProjectName);

            if (Config.IsPresent)
            {

                WriteConfig(Factory.CreateVirtualNetworksClient().GetConfig(projectId).Value);
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


            ListOutput(Factory.CreateVirtualNetworksClient().List(projectId: projectId));

        }

        private void WriteConfig(VirtualNetworkConfiguration config)
        {
            var networkConfig = ProjectNetworksConfigJsonSerializer.Deserialize(
                config.Configuration);

            var yaml = ProjectNetworksConfigYamlSerializer.Serialize(networkConfig);
            WriteObject(yaml);
        }

    }

}