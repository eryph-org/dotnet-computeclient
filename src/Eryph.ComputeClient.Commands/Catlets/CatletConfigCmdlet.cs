using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Host;
using Eryph.ConfigModel.Json;
using Eryph.ConfigModel.Catlets;
using Eryph.ConfigModel.Variables;
using Eryph.ConfigModel.Yaml;
using YamlDotNet.Core;

namespace Eryph.ComputeClient.Commands.Catlets
{
    public class CatletConfigCmdlet : CatletCmdLet
    {

        protected static CatletConfig DeserializeConfigString(string configString)
        {
            configString = configString.Trim();
            configString = configString.Replace("\r\n", "\n");

            if (configString.StartsWith("{") && configString.EndsWith("}"))
                return CatletConfigDictionaryConverter.Convert(ConfigModelJsonSerializer.DeserializeToDictionary(configString));

            //YAML
            try
            {
                return CatletConfigYamlSerializer.Deserialize(configString);
            }
            catch (YamlException ex)
            {
                throw ex;
            }

        }

        protected void PopulateVariables(CatletConfig catletConfig)
        {
            if (catletConfig.Variables is not { Length: > 0 })
                return;

            var fieldDescriptions = catletConfig.Variables.Select(v =>
            {
                var fieldDescription = new FieldDescription(v.Name)
                {
                    IsMandatory = v.Required.GetValueOrDefault(),
                    DefaultValue = v.Value is not null ? new PSObject(v.Value) : null,
                };
                fieldDescription.SetParameterType(GetType(v.Type.GetValueOrDefault(VariableType.String)));
                return fieldDescription;
            }).ToList();

            try
            {
                var results = Host.UI.Prompt(
                    "Catlet Parameters",
                    "Please provide values for catlet parameters",
                    new Collection<FieldDescription>(fieldDescriptions));

                foreach (var result in results)
                {
                    var variable = catletConfig.Variables.FirstOrDefault(v => v.Name == result.Key);
                    if (variable is not null)
                    {
                        variable.Value = result.Value.ToString();
                    }
                }
            }
            // TODO proper exception type
            catch (Exception ex)
            {
                // The prompt will fail if the Powershell session is not
                // interactive. Unfortunately, there is no easy way to
                // reliably detect a non-interactive session. Hence, we
                // just catch the exception and continue.
            }
        }

        private static Type GetType(VariableType variableType) =>
        variableType switch
        {
            VariableType.String => typeof(string),
            VariableType.Number => typeof(decimal),
            VariableType.Boolean => typeof(bool),
            _ => throw new ArgumentException(
                $"The variable type {variableType} is not supported.",
                nameof(variableType))
        };
    }
}
