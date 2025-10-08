using Eryph.ComputeClient.Commands.Catlets;
using Eryph.ComputeClient.Models;
using Eryph.ConfigModel.Catlets;
using Eryph.ConfigModel.Json;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace Eryph.ComputeClient.Commands.CatletSpecifications;

[PublicAPI]
[Cmdlet(VerbsCommon.New, "CatletSpecification")]
[OutputType(typeof(Operation))]
public class NewCatletSpecification : CatletSpecificationCmdlet
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
    public string Name { get; set; }

    [Parameter]
    [ValidateNotNullOrEmpty]
    public string ProjectName { get; set; }

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
        //string config = Config;

        //if (!string.IsNullOrWhiteSpace(input))
        //{
        //    config = DeserializeConfigString(input);
        //}

        //if (!string.IsNullOrWhiteSpace(Parent))
        //{
        //    config = new CatletConfig
        //    {
        //        Parent = Parent
        //    };
        //}

        if (string.IsNullOrEmpty(input))
            return;

        //if (config.ConfigType is not (null or CatletConfigType.Specification))
        //    throw new InvalidOperationException($"The provided configuration has the type {config.ConfigType}. "
        //                                        + "Please use a fresh configuration when creating a new catlet.");


        var projectName = string.IsNullOrWhiteSpace(ProjectName) ? "default" : ProjectName;
        var projectId = GetProjectId(projectName);
        //var existingCatlets = Factory.CreateCatletsClient().List(projectId: projectId).ToList();
        //if (existingCatlets.Any(c => string.Equals(c.Name, catletName, StringComparison.OrdinalIgnoreCase)))
        //    throw new InvalidOperationException($"A catlet with name '{catletName}' already exists in project '{projectName}'. Catlet names must be unique within a project.");

        //if (!PopulateVariables(config, Variables, SkipVariablesPrompt, false))
        //    return;

        //var serializedConfig = CatletConfigJsonSerializer.SerializeToElement(config);

        WaitForOperation(Factory.CreateCatletSpecificationsClient().Create(
                new NewCatletSpecificationRequest(Guid.Parse(projectId), Name, input)
                {
                    CorrelationId = Guid.NewGuid(),
                }),
            NoWait,
            true);
    }
}
