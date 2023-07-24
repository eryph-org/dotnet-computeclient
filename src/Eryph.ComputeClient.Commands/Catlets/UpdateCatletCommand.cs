﻿using System;
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
    [OutputType(typeof(Operation), typeof(Catlet), typeof(VirtualCatlet))]
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
        public SwitchParameter Wait
        {
            get => _wait;
            set => _wait = value;
        }

        private bool _wait;


        protected override void ProcessRecord()
        {
            foreach (var id in Id)
            {
                var config = DeserializeConfigString(Config);
                WaitForOperation(Factory.CreateCatletsClient().Update(new UpdateCatletRequest(Guid.NewGuid(),
                        JsonSerializer.SerializeToElement(config), id))
                    , _wait, true);
            }

        }

    }
}