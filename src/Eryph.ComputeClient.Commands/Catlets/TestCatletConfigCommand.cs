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
    public class TestCatletConfigCommand : CatletConfigCmdlet
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
        public SwitchParameter NoWait { get; set; }

        [Parameter]
        [ValidateNotNullOrEmpty]
        public string ProjectName { get; set; }

        [Parameter(ParameterSetName = "Parent")]
        [ValidateNotNullOrEmpty]
        public string Parent { get; set; }

        [Parameter]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        [Parameter]
        public Hashtable Variables { get; set; }

        [Parameter]
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

            if (!string.IsNullOrWhiteSpace(ProjectName))
                config.Project = ProjectName;


            if (!string.IsNullOrWhiteSpace(Name))
                config.Name = Name;

            if (!PopulateVariables(config, Variables, SkipVariablesPrompt))
                return;

            var serializedConfig = CatletConfigJsonSerializer.SerializeToElement(config);

            var operation = Factory.CreateCatletsClient().ExpandConfig(
                new ExpandCatletConfigRequest(serializedConfig)
                {
                    CorrelationId = Guid.NewGuid(),
                });
            if (NoWait)
            {
                WriteObject(operation);
                return;
            }

            WaitForOperation(operation, (op) =>
            {
                if (op.Result is CatletConfigOperationResult ccor)
                {
                    var expandedConfig = CatletConfigJsonSerializer.Deserialize(ccor.Configuration);
                    var yaml = CatletConfigYamlSerializer.Serialize(expandedConfig);
                    WriteObject(yaml);
                }

            });
        }
    }
}
