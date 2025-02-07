using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Host;
using Eryph.ConfigModel.Json;
using Eryph.ConfigModel.Catlets;
using Eryph.ConfigModel.Variables;
using Eryph.ConfigModel.Yaml;
using YamlDotNet.Core;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Eryph.ComputeClient.Commands.Catlets
{
    public class CatletConfigCmdlet : CatletCmdLet
    {

        protected static CatletConfig DeserializeConfigString(string configString)
        {
            configString = configString.Trim();
            configString = configString.Replace("\r\n", "\n");

            if (configString.StartsWith("{") && configString.EndsWith("}"))
                return CatletConfigJsonSerializer.Deserialize(configString);

            
            return CatletConfigYamlSerializer.Deserialize(configString);

        }

        /// <summary>
        /// Populates the variables in the given <paramref name="catletConfig"/> by
        /// applying the values from the <paramref name="variables"/> hashtable and
        /// interactively asking the user for the values. Returns <see langword="false"/>
        /// when the user aborted the interactive prompt and <see langword="true"/> otherwise.
        /// </summary>
        protected bool PopulateVariables(
            CatletConfig catletConfig,
            Hashtable variables,
            bool skipVariablesPrompt,
            bool showSecrets)
        {
            if (catletConfig.Variables is not { Length: > 0 })
                return true;

            ApplyVariablesFromParameter(catletConfig.Variables, variables);
            
            return skipVariablesPrompt || ReadVariablesFromInput(catletConfig.Variables, showSecrets);
        }

        private static void ApplyVariablesFromParameter(VariableConfig[] variableConfigs, Hashtable variables)
        {
            if (variables is not { Count: > 0 })
                return;

            var variablesByKey = variables.Cast<DictionaryEntry>()
                .Where(entry => entry.Key is string)
                .ToLookup(e => (string)e.Key, e => e.Value.ToString(), StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.First(), StringComparer.OrdinalIgnoreCase);

            foreach (var variableConfig in variableConfigs)
            {
                if (!string.IsNullOrWhiteSpace(variableConfig.Name)
                    && variablesByKey.TryGetValue(variableConfig.Name, out var providedValue))
                {
                    variableConfig.Value = providedValue;
                }
            }
        }

        private bool ReadVariablesFromInput(
            VariableConfig[] variableConfigs,
            bool showSecrets)
        {
            var anyRequired = variableConfigs.Any(vc =>
                vc.Required.GetValueOrDefault()
                && string.IsNullOrWhiteSpace(vc.Value));

            try
            {
                if (showSecrets)
                {
                    Host.UI.WriteWarningLine("Secrets will be displayed in clear text.");
                }

                var choices = new Collection<ChoiceDescription>()
                {
                    new("&Yes", "You will be asked to provide values for all variables."),
                    new("&No", "You will not be asked for any values."),
                };

                if(anyRequired)
                {
                    choices.Add(new ChoiceDescription("&Required only",
                        "You will only be asked for the values of variables which are required and do not have a value yet."));
                }

                // The prompt for choice will fail when the Powershell session
                // is not interactive. This also serves as a check whether we
                // are in an interactive session.
                var choice = Host.UI.PromptForChoice(
                    "Catlet variables",
                    "Would you like to provide variable values interactively?"
                    + (anyRequired
                        ? " Some required variables are missing values. The deployment will fail if you do not specify them."
                        : ""),
                    choices,
                    anyRequired ? 2 : 1);

                // The prompt returns -1 when the user cancels the prompt (e.g. Ctrl+C).
                if (choice is -1 or 1)
                    return choice != -1;

                var requiredOnly = choice == 2;

                var configsToRead = variableConfigs.Where(vc =>
                    !requiredOnly || vc.Required.GetValueOrDefault() && string.IsNullOrWhiteSpace(vc.Value));

                foreach (var variableConfig in configsToRead)
                {
                    var result = ReadVariableFromInput(variableConfig, showSecrets);
                    if (result is null)
                        return false;

                    variableConfig.Value = result;
                }
            }
            catch (Exception ex) when (ex is PSInvalidOperationException or HostException)
            {
                // The prompt for choice will fail if the Powershell session is not
                // interactive. Unfortunately, there is no easy way to reliably detect
                // a non-interactive session. Hence, we just catch the exception and continue.
            }

            return true;
        }

        private string ReadVariableFromInput(
            VariableConfig config,
            bool showSecrets)
        {
            var prompt = PreparePrompt(config);
            
            while (true)
            {
                var result = ReadFromInput(prompt, (config.Secret ?? false) && !showSecrets);

                // Null indicates that the user cancelled the input (e.g. with Ctrl+C).
                // In this case, we return null and cancel the whole operation.
                if (result is null)
                    return null;

                if (string.IsNullOrEmpty(result))
                    result = config.Value;
                
                if (IsValueValid(config, result))
                    return result;

                Host.UI.WriteLine("The provided value is invalid. Please try again.");
            }
        }

        private static string PreparePrompt(VariableConfig config)
        {
            var prompt = config.Type switch
            {
                VariableType.Boolean => $"[{config.Type} (true/false)]",
                _ => $"[{config.Type ?? VariableType.String}]",
            };

            prompt += $" {config.Name}";

            if (!string.IsNullOrWhiteSpace(config.Value))
            {
                prompt += config.Secret switch
                {
                    true => " (***)",
                    _ => $" ({config.Value})"
                };
            }

            return prompt + ": ";
        }

        private static bool IsValueValid(VariableConfig config, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return !config.Required.GetValueOrDefault();

            return (config.Type ?? VariableType.String) switch
            {
                VariableType.Boolean => Regex.IsMatch(
                    value,
                    VariableValueRegex.Boolean,
                    RegexOptions.None,
                    TimeSpan.FromSeconds(1)),
                VariableType.Number => Regex.IsMatch(
                    value,
                    VariableValueRegex.Number,
                    RegexOptions.None,
                    TimeSpan.FromSeconds(1)),
                _ => true
            };
        }

        private string ReadFromInput(string prompt, bool secret)
        {
            Host.UI.Write(prompt);
            return secret ? ReadSecret() : Host.UI.ReadLine();
        }

        private string ReadSecret()
        {
            var result = Host.UI.ReadLineAsSecureString();
            var pointer = Marshal.SecureStringToBSTR(result);
            try
            {
                return Marshal.PtrToStringBSTR(pointer);
            }
            finally
            {
                Marshal.ZeroFreeBSTR(pointer);
            }
        }
    }
}
