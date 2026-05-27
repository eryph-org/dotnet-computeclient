using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Eryph.ComputeClient.Models;
using Eryph.ConfigModel.Json;
using Eryph.ConfigModel.Networks;
using Eryph.ConfigModel.Yaml;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.Networks
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Get, "VNetwork", DefaultParameterSetName = "list")]
    [OutputType(typeof(VirtualNetwork), ParameterSetName = new[] { "get" })]
    [OutputType(typeof(VirtualNetwork), ParameterSetName = new[] { "list" })]
    [OutputType(typeof(string), ParameterSetName = new[] { "getconfig" })]
    public class GetVNetworksCommand : NetworkCmdLet
    {
        [Parameter(
            ParameterSetName = "get",
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string[] Id { get; set; }

        [Parameter(
            ParameterSetName = "getconfig",
            Mandatory = true)]
        public SwitchParameter Config { get; set; }

        [Parameter(
            ParameterSetName = "list",
            Position = 0)]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        [Parameter(ParameterSetName = "list")]
        [ValidateNotNullOrEmpty]
        public string Environment { get; set; }

        [Parameter(
            ParameterSetName = "get",
            ValueFromPipelineByPropertyName = true)]
        [Parameter(
            ParameterSetName = "getconfig",
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        [Parameter(
            ParameterSetName = "list",
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
                    if (Stopping) break;
                    if (TryGetById(id, GetSingleNetwork, "virtual network", out var network))
                        WriteObject(network);
                }

                return;
            }

            // Network names are unique per project + environment, so allow narrowing by
            // environment. The environment filter is applied to the listing; an explicit
            // id (GUID) ignores it.
            IEnumerable<VirtualNetwork> networks = Factory.CreateVirtualNetworksClient().List(projectId: projectId);
            if (!string.IsNullOrWhiteSpace(Environment))
                networks = networks.Where(n => string.Equals(n.Environment, Environment, StringComparison.OrdinalIgnoreCase));

            WriteByNameOrId(Name, GetSingleNetwork, () => networks, n => n.Name, "virtual network");
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