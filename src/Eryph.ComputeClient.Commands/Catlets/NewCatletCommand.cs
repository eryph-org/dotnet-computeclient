using System;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Text.Json;
using Azure.Core.Serialization;
using Eryph.ClientRuntime;
using Eryph.ComputeClient.Models;
using Eryph.ConfigModel.Catlets;
using Eryph.ConfigModel.Json;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.Catlets
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.New, "Catlet")]
    [OutputType(typeof(Operation), typeof(Catlet), typeof(VirtualCatlet))]
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
        public SwitchParameter Wait
        {
            get => _wait;
            set => _wait = value;
        }

        [Parameter]
        [ValidateNotNullOrEmpty]
        public string Project { get; set; }

        [Parameter(ParameterSetName = "Image")]
        [ValidateNotNullOrEmpty]
        public string Image { get; set; }

        [Parameter]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        private bool _wait;
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

            if (!string.IsNullOrWhiteSpace(Image))
            {
                config = new CatletConfig
                {
                    VCatlet = new VirtualCatletConfig
                    {
                        Image = Image
                    }
                };
            }

            if (config == null)
                return;

            if (!string.IsNullOrWhiteSpace(Project))
                config.Project = Project;


            if (!string.IsNullOrWhiteSpace(Name))
                config.Name = Name;

            WaitForOperation(Factory.CreateCatletsClient()
                .Create(
                    new NewCatletRequest(Guid.NewGuid(),
                       JsonSerializer.SerializeToElement(config))), _wait, true);
        }
    }



}