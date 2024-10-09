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


        [Parameter(Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string ProjectName { get; set; }

        [Parameter]
        public SwitchParameter Force { get; set; }

        [Parameter]
        public SwitchParameter NoWait { get; set; }

        private readonly StringBuilder _input = new();

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

            if (string.IsNullOrWhiteSpace(input))
                return;

            var config = DeserializeConfigString(input);
            if (config is null)
                return;

            var projectId = GetProjectId(config.Project);


            if (config.Project != ProjectName
                && !Force
                && !ShouldContinue($"The network configuration was exported from project '{config.Project}' but will be imported to '{ProjectName}'. Continue?", "Warning!"))
            {
                return;
            }
                
            config.Project = ProjectName;
            var configJson = JsonSerializer.SerializeToElement(config, ConfigModelJsonSerializer.DefaultOptions);
            
            WaitForOperation(
                Factory.CreateVirtualNetworksClient().UpdateConfig(
                    projectId,
                    new UpdateProjectNetworksRequestBody(configJson)
                    {
                        CorrelationId = Guid.NewGuid(),
                    }),
                NoWait, false);
        }
    }
}
