﻿using System;
using System.Collections;
using System.Management.Automation;
using System.Text.Json;
using Eryph.ClientRuntime;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;
using Operation = Eryph.ComputeClient.Models.Operation;

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
        public SwitchParameter NoWait
        {
            get => _nowait;
            set => _nowait = value;
        }

        private bool _nowait;

        [Parameter]
        public Hashtable Variables { get; set; }

        [Parameter]
        public SwitchParameter SkipVariablesPrompt { get; set; }

        protected override void ProcessRecord()
        {
            foreach (var id in Id)
            {
                var config = DeserializeConfigString(Config);

                if (!PopulateVariables(config, Variables, SkipVariablesPrompt))
                    continue;

                WaitForOperation(Factory.CreateCatletsClient().Update(id, new UpdateCatletRequestBody(Guid.NewGuid(),
                        JsonSerializer.SerializeToElement(config)))
                    , _nowait, true);
            }
        }
    }
}