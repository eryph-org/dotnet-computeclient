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
using Org.BouncyCastle.Asn1;

namespace Eryph.ComputeClient.Commands.Catlets
{
    [PublicAPI]
    [Cmdlet(VerbsDiagnostic.Test, "CatletConfig")]
    [OutputType(typeof(Operation), typeof(string))]
    [OutputType(typeof(CatletConfigValidationResult), ParameterSetName = ["QuickConfig", "QuickInputObject"])]
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

        [Parameter(ParameterSetName = "Config", Mandatory = true)]
        [Parameter(ParameterSetName = "ConfigAndId", Mandatory = true)]
        [Parameter(ParameterSetName = "QuickConfig", Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Config { get; set; }

        [Parameter(ParameterSetName = "InputObjectAndId", Mandatory = true)]
        [Parameter(ParameterSetName = "ConfigAndId", Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Id { get; set; }

        [Parameter(ParameterSetName = "Config")]
        [Parameter(ParameterSetName = "ConfigAndId")]
        [Parameter(ParameterSetName = "InputObject")]
        [Parameter(ParameterSetName = "InputObjectAndId")]
        public SwitchParameter NoWait { get; set; }

        [Parameter(ParameterSetName = "QuickConfig", Mandatory = true)]
        [Parameter(ParameterSetName = "QuickInputObject", Mandatory = true)]
        public SwitchParameter Quick { get; set; }

        [Parameter(ParameterSetName = "Config")]
        [Parameter(ParameterSetName = "InputObject")]
        [ValidateNotNullOrEmpty]
        public string ProjectName { get; set; }

        [Parameter(ParameterSetName = "Config")]
        [Parameter(ParameterSetName = "InputObject")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        [Parameter(ParameterSetName = "Config")]
        [Parameter(ParameterSetName = "ConfigAndId")]
        [Parameter(ParameterSetName = "InputObject")]
        [Parameter(ParameterSetName = "InputObjectAndId")]
        public Hashtable Variables { get; set; }

        [Parameter(ParameterSetName = "Config")]
        [Parameter(ParameterSetName = "ConfigAndId")]
        [Parameter(ParameterSetName = "InputObject")]
        [Parameter(ParameterSetName = "InputObjectAndId")]
        public SwitchParameter SkipVariablesPrompt { get; set; }

        [Parameter(ParameterSetName = "Config")]
        [Parameter(ParameterSetName = "ConfigAndId")]
        [Parameter(ParameterSetName = "InputObject")]
        [Parameter(ParameterSetName = "InputObjectAndId")]
        public SwitchParameter ShowSecrets { get; set; }

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

            if (!PopulateVariables(config, Variables, SkipVariablesPrompt, ShowSecrets))
                return;

            var serializedConfig = CatletConfigJsonSerializer.SerializeToElement(config);

            var operation = string.IsNullOrWhiteSpace(Id)
                ? client.ExpandNewConfig(new ExpandNewCatletConfigRequest(serializedConfig)
                {   
                    CorrelationId = Guid.NewGuid(),
                    ShowSecrets = ShowSecrets,
                })
                : client.ExpandConfig(Id, new ExpandCatletConfigRequestBody(serializedConfig)
                {
                    CorrelationId = Guid.NewGuid(),
                    ShowSecrets = ShowSecrets,
                });

            if (NoWait)
            {
                WriteObject(operation);
                return;
            }

            WaitForOperation(operation, op =>
            {
                if (op.Result is not CatletConfigOperationResult configResult)
                    return;
                
                var expandedConfig = CatletConfigJsonSerializer.Deserialize(configResult.Configuration);
                var yaml = CatletConfigYamlSerializer.Serialize(expandedConfig);
                WriteObject(yaml);
            });
        }
    }
}
