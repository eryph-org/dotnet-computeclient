using System;
using System.Collections;
using System.Management.Automation;
using Eryph.ComputeClient.Models;
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
                var configJson = CatletConfigJsonSerializer.SerializeToElement(config);

                if (!NoWait)
                {
                    WaitForOperation(client.ExpandConfig(
                        id,
                        new ExpandCatletConfigRequestBody(configJson)
                        {
                            CorrelationId = Guid.NewGuid(),
                        }));
                }

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