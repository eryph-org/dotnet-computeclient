using System;
using System.Collections;
using System.Management.Automation;
using Eryph.ComputeClient.Models;
using Eryph.ConfigModel.Catlets;
using Eryph.ConfigModel.Json;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.Catlets
{
    [PublicAPI]
    [Cmdlet(VerbsData.Update, "Catlet")]
    [OutputType(typeof(Operation), typeof(Catlet), typeof(Catlet))]
    public class UpdateCatletCommand : CatletConfigCmdlet
    {
        [Parameter(
            Position = 0,
            ValueFromPipeline = true,
            Mandatory = true,
            ValueFromPipelineByPropertyName = true)]
        public string[] Id { get; set; }

        [Parameter]
        public string Config { get; set; }

        [Parameter]
        public SwitchParameter NoWait { get; set; }

        protected override void ProcessRecord()
        {
            var client = Factory.CreateCatletsClient();

            foreach (var id in Id)
            {
                var config = DeserializeConfigString(Config);

                if (config.ConfigType is not CatletConfigType.Instance)
                {
                    WriteError(new ErrorRecord(
                        new InvalidOperationException("The catlet configuration is not an instance configuration "
                                                      + "and cannot be used to update an existing catlet. Please "
                                                      + "use Get-Catlet -Config to get a configuration for update purposes."),
                        "ConfigIsNotInstanceSpecific",
                        ErrorCategory.InvalidOperation,
                        id));
                }

                if (config.Fodder is not null || config.Variables is not null)
                    WriteWarning("The fodder and variables cannot be changed when updating a catlet. The provided data will be ignored.");

                config.Fodder = null;
                config.Variables = null;

                var configJson = CatletConfigJsonSerializer.SerializeToElement(config);

                WaitForOperation(client.Update(
                        id,
                        new UpdateCatletRequestBody(configJson)
                        {
                            CorrelationId = Guid.NewGuid(),
                        }),
                    NoWait, true);
            }
        }
    }
}