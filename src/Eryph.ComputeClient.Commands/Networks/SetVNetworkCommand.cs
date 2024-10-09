using System;
using System.Management.Automation;
using System.Text;
using System.Text.Json;
using Eryph.ComputeClient.Models;
using Eryph.ConfigModel.Json;
using Eryph.ConfigModel.Networks;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.Networks
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Set, "VNetwork")]
    [OutputType(typeof(Operation))]
    public class SetVNetworkCommand : NetworkConfigCmdlet
    {
        [Parameter(
            ParameterSetName = "InputObject",
            Position = 0,
            ValueFromPipeline = true,
            Mandatory = true)]
        [AllowEmptyString]
        public string[] InputObject { get; set; }

        [Parameter(
            ParameterSetName = "Config",
            Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Config { get; set; }

        [Parameter]
        public SwitchParameter NoWait
        {
            get => _nowait;
            set => _nowait = value;
        }

        [Parameter(Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string ProjectName { get; set; }

        private bool _nowait;
        private StringBuilder _input = new StringBuilder();


        protected override void ProcessRecord()
        {
            if (InputObject != null)
            {
                foreach (var input in InputObject)
                {
                    _input.AppendLine($"{input}");

                }
            }

            if (Config != null)
                _input.Append(Config);

        }

        protected override void EndProcessing()
        {
            var input = _input.ToString();
            ProjectNetworksConfig config = null;

            if (!string.IsNullOrWhiteSpace(input))
            {
                config = DeserializeConfigString(input);
            }

            if (config == null)
                return;

            var projectId = GetProjectId(ProjectName);

            if (!string.IsNullOrWhiteSpace(ProjectName))
                config.Project = ProjectName;
            
            var configJson = JsonSerializer.SerializeToElement(config, ConfigModelJsonSerializer.DefaultOptions);
            WaitForOperation(
                Factory.CreateVirtualNetworksClient().UpdateConfig(
                    projectId,
                    new UpdateProjectNetworksRequestBody(configJson)
                    {
                        CorrelationId = Guid.NewGuid(),
                    }),
                _nowait, false);
        }
    }



}