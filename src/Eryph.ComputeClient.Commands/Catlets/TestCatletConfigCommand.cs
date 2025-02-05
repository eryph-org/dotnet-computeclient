using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using Eryph.ComputeClient.Models;
using Eryph.ConfigModel.Catlets;
using Eryph.ConfigModel.Json;
using Eryph.ConfigModel.Yaml;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.Catlets
{
    [PublicAPI]
    [Cmdlet(VerbsDiagnostic.Test, "CatletConfig")]
    [OutputType(typeof(Operation), typeof(string))]
    [OutputType(typeof(bool), ParameterSetName = ["Quick"])]
    public class TestCatletConfigCommand : CatletConfigCmdlet
    {
        [Parameter(
            ParameterSetName = "InputObject",
            Position = 0,
            ValueFromPipeline = true,
            Mandatory = true)]
        [Parameter(
            ParameterSetName = "InputObjectAndId",
            Position = 0,
            ValueFromPipeline = true,
            Mandatory = true)]
        [Parameter(
            ParameterSetName = "QuickInputObject",
            Position = 0,
            ValueFromPipeline = true,
            Mandatory = true)]
        [AllowEmptyString]
        public string[] InputObject { get; set; }

        [Parameter(
            ParameterSetName = "Config",
            Mandatory = true)]
        [Parameter(
            ParameterSetName = "ConfigAndId",
            Mandatory = true)]
        [Parameter(ParameterSetName = "QuickConfig")]
        [ValidateNotNullOrEmpty]
        public string Config { get; set; }

        [Parameter(
            ParameterSetName = "InputObjectAndId",
            Mandatory = true)]
        [Parameter(
            ParameterSetName = "ConfigAndId",
            Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Id { get; set; }

        [Parameter(ParameterSetName = "Config")]
        [Parameter(ParameterSetName = "ConfigAndId")]
        [Parameter(ParameterSetName = "InputObject")]
        [Parameter(ParameterSetName = "InputObjectAndId")]
        [Parameter(ParameterSetName = "Parent")]
        public SwitchParameter NoWait { get; set; }

        [Parameter(ParameterSetName = "QuickConfig")]
        [Parameter(ParameterSetName = "QuickInputObject")]
        public SwitchParameter Quick { get; set; }

        [Parameter(ParameterSetName = "Config")]
        [Parameter(ParameterSetName = "InputObject")]
        [Parameter(ParameterSetName = "Parent")]
        [ValidateNotNullOrEmpty]
        public string ProjectName { get; set; }

        [Parameter(ParameterSetName = "Parent")]
        [ValidateNotNullOrEmpty]
        public string Parent { get; set; }

        [Parameter(ParameterSetName = "Config")]
        [Parameter(ParameterSetName = "InputObject")]
        [Parameter(ParameterSetName = "Parent")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        [Parameter(ParameterSetName = "Config")]
        [Parameter(ParameterSetName = "ConfigAndId")]
        [Parameter(ParameterSetName = "InputObject")]
        [Parameter(ParameterSetName = "InputObjectAndId")]
        [Parameter(ParameterSetName = "Parent")]
        public Hashtable Variables { get; set; }

        [Parameter(ParameterSetName = "Config")]
        [Parameter(ParameterSetName = "ConfigAndId")]
        [Parameter(ParameterSetName = "InputObject")]
        [Parameter(ParameterSetName = "InputObjectAndId")]
        [Parameter(ParameterSetName = "Parent")]
        public SwitchParameter SkipVariablesPrompt { get; set; }

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
            CatletConfig config = null;
            
            if (!string.IsNullOrWhiteSpace(input))
            {
                config = DeserializeConfigString(input);
            }

            if (!string.IsNullOrWhiteSpace(Parent))
            {
                config = new CatletConfig
                {
                    Parent = Parent
                };
            }

            if (config == null)
                return;

            var client = Factory.CreateCatletsClient();
            if (Quick)
            {
                var result = client.ValidateConfig(new ValidateConfigRequest(
                    CatletConfigJsonSerializer.SerializeToElement(config)));
                WriteObject(result.Value);
                return;
            }

            if (!string.IsNullOrWhiteSpace(ProjectName))
                config.Project = ProjectName;

            if (!string.IsNullOrWhiteSpace(Name))
                config.Name = Name;

            if (!PopulateVariables(config, Variables, SkipVariablesPrompt))
                return;

            var serializedConfig = CatletConfigJsonSerializer.SerializeToElement(config);

            var operation = string.IsNullOrWhiteSpace(Id)
                ? client.ExpandNewConfig(new ExpandNewCatletConfigRequest(serializedConfig)
                {   
                    CorrelationId = Guid.NewGuid(),
                })
                : client.ExpandConfig(Id, new ExpandCatletConfigRequestBody(serializedConfig)
                {
                    CorrelationId = Guid.NewGuid(),
                });

            if (NoWait)
            {
                WriteObject(operation);
                return;
            }

            WaitForOperation(operation, op =>
            {
                if (op.Result is CatletConfigOperationResult configResult)
                {
                    var expandedConfig = CatletConfigJsonSerializer.Deserialize(configResult.Configuration);
                    var yaml = CatletConfigYamlSerializer.Serialize(expandedConfig);
                    WriteObject(yaml);
                }
            });
        }
    }
}
