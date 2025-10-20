using System;
using System.Collections;
using System.Linq;
using System.Management.Automation;
using System.Text;
using Eryph.ComputeClient.Models;
using Eryph.ConfigModel.Catlets;
using Eryph.ConfigModel.Json;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.Catlets
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.New, "Catlet")]
    [OutputType(typeof(Operation), typeof(Catlet))]
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

            if (config is null)
                return;

            if (config.ConfigType is not (null or CatletConfigType.Specification))
                throw new InvalidOperationException($"The provided configuration has the type {config.ConfigType}. "
                                                    + "Please use a fresh configuration when creating a new catlet.");

            if (!string.IsNullOrWhiteSpace(ProjectName))
                config.Project = ProjectName;

            if (!string.IsNullOrWhiteSpace(Name))
                config.Name = Name;

            var catletName = string.IsNullOrWhiteSpace(config.Name) ? "catlet" : config.Name;
            var projectName = string.IsNullOrWhiteSpace(config.Project) ? "default" : config.Project;
            var projectId = GetProjectId(projectName);
            var existingCatlets = Factory.CreateCatletsClient().List(projectId: projectId).ToList();
            if (existingCatlets.Any(c => string.Equals(c.Name, catletName, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException($"A catlet with name '{catletName}' already exists in project '{projectName}'. Catlet names must be unique within a project.");

            if (!PopulateVariables(config, Variables, SkipVariablesPrompt, false))
                return;

            var serializedConfig = CatletConfigJsonSerializer.SerializeToElement(config);
            
            WaitForOperation(Factory.CreateCatletsClient().Create(
                    new NewCatletRequest(serializedConfig)
                    {
                        CorrelationId = Guid.NewGuid(),
                    }),
                NoWait,
                true);
        }
    }
}
