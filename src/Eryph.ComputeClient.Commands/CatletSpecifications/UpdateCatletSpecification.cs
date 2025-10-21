using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using Eryph.ComputeClient.Models;

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

    [Parameter(Mandatory = true)]
    [ValidateNotNullOrEmpty]
    public string Id { get; set; }

    [Parameter]
    [ValidateNotNullOrEmpty]
    public string Comment { get; set; }

    [Parameter]
    public SwitchParameter NoWait { get; set; }

    [Parameter]
    [ValidateNotNullOrEmpty]
    public string Name { get; set; }

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

        WaitForOperation(Factory.CreateCatletSpecificationsClient().Update(
                Id,
                new UpdateCatletSpecificationRequestBody(input)
                {
                    CorrelationId = Guid.NewGuid(),
                    Comment = Comment,
                    Name = Name,
                }),
            NoWait,
            true);
    }
}
