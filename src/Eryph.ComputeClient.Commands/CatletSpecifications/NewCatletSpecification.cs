using System;
using System.Management.Automation;
using System.Text;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.CatletSpecifications;

[PublicAPI]
[Cmdlet(VerbsCommon.New, "CatletSpecification")]
[OutputType(typeof(Operation), typeof(CatletSpecification))]
public class NewCatletSpecification : CatletSpecificationCmdlet
{
    [Parameter(
        ParameterSetName = "InputObject",
        Position = 0,
        ValueFromPipeline = true,
        Mandatory = true)]
    [AllowEmptyString]
    public string[] InputObject { get; set; }

    [Parameter(ParameterSetName = "Config", Mandatory = true)]
    [ValidateNotNullOrEmpty]
    public string Config { get; set; }

    [Parameter]
    [ValidateNotNullOrEmpty]
    public string Comment { get; set; }

    [Parameter]
    [ValidateNotNullOrEmpty]
    public string ProjectName { get; set; }

    [Parameter]
    public SwitchParameter Json { get; set; }

    [Parameter]
    public string[] Architectures { get; set; }

    [Parameter]
    public SwitchParameter NoWait { get; set; }

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
        if (string.IsNullOrEmpty(input))
            return;

        var projectName = string.IsNullOrWhiteSpace(ProjectName) ? "default" : ProjectName;
        var projectId = GetProjectId(projectName);

        var config = new CatletSpecificationConfig(
            Json.ToBool() ? "application/json" : "application/yaml",
            input);

        WaitForOperation(Factory.CreateCatletSpecificationsClient().Create(
                new NewCatletSpecificationRequest(Guid.Parse(projectId), config)
                {
                    Comment = Comment,
                    CorrelationId = Guid.NewGuid(),
                    Architectures = Architectures,
                }),
            NoWait,
            true);
    }
}
