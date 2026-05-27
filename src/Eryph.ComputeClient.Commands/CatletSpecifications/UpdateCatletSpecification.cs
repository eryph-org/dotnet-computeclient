using System;
using System.Management.Automation;
using System.Text;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.CatletSpecifications;

[PublicAPI]
[Cmdlet(VerbsData.Update, "CatletSpecification")]
[OutputType(typeof(Operation))]
public class UpdateCatletSpecification : CatletSpecificationCmdlet
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

    // Not positional (unlike the other action cmdlets' -Id): position 0 is taken by
    // -InputObject, the specification content. The target is given via -Id / -Name.
    [Parameter(Mandatory = true)]
    [ValidateNotNullOrEmpty]
    [Alias("Name")]
    public string Id { get; set; }

    [Parameter]
    [ValidateNotNullOrEmpty]
    public string Comment { get; set; }

    [Parameter]
    public SwitchParameter Json { get; set; }

    [Parameter]
    public string[] Architectures { get; set; }

    [Parameter]
    public SwitchParameter NoWait { get; set; }

    [Parameter]
    [ValidateNotNullOrEmpty]
    public string ProjectName { get; set; }

    private readonly StringBuilder _input = new StringBuilder();

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

        var config = new CatletSpecificationConfig(
            Json.ToBool() ? "application/json" : "application/yaml",
            input);

        foreach (var specification in ResolveActionTargets(Id, ProjectName, GetSingleCatletSpecification,
                     projectId => Factory.CreateCatletSpecificationsClient().List(projectId: projectId),
                     s => s.Name, "catlet specification"))
        {
            if (Stopping) break;

            WaitForOperation(Factory.CreateCatletSpecificationsClient().Update(
                    specification.Id,
                    new UpdateCatletSpecificationRequestBody(config)
                    {
                        CorrelationId = Guid.NewGuid(),
                        Comment = Comment,
                        Architectures = Architectures,
                    }),
                NoWait,
                true);
        }
    }
}
