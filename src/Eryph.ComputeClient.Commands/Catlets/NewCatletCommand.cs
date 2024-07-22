using System;
using System.Collections;
using System.Management.Automation;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Eryph.ComputeClient.Models;
using Eryph.ConfigModel.Catlets;
using Eryph.ConfigModel.Json;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.Catlets
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.New, "Catlet")]
    [OutputType(typeof(Operation), typeof(Catlet), typeof(Catlet))]
    public class NewCatletCommand : CatletConfigCmdlet
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
            get => _noWait;
            set => _noWait = value;
        }

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

        private bool _noWait;
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

            var serializedConfig = JsonSerializer.SerializeToElement(config, ConfigModelJsonSerializer.DefaultOptions);

            WaitForOperation(
                Factory.CreateCatletsClient().Create(
                    new NewCatletRequest(Guid.NewGuid(), serializedConfig)),
                _noWait,
                true);
        }
    }
}
