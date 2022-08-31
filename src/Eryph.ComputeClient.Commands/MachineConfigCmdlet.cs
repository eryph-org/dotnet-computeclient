using System;
using System.Collections.Generic;
using Eryph.ConfigModel.Json;
using Eryph.ConfigModel.Machine;
using Newtonsoft.Json.Linq;
using SharpYaml;
using SharpYaml.Serialization;

namespace Eryph.ComputeClient.Commands
{
    public class MachineConfigCmdlet : ComputeCmdLet
    {
        protected static MachineConfig DeserializeConfigString(string configString)
        {
            AppDomain.CurrentDomain.GetAssemblies();
            IDictionary<string, object> configDictionary;
            var looseMode = false;
            configString = configString.Trim();
            configString = configString.Replace("\r\n", "\n");

            if (configString.StartsWith("{") && configString.EndsWith("}"))
                configDictionary = ConfigModelJsonSerializer.DeserializeToDictionary(configString);
                
            else
            {

                //YAML
                try
                {
                    var deserializer = new Serializer();
                    configDictionary = deserializer.Deserialize<Dictionary<string, object>>(configString);
                    looseMode = true;
                }
                catch (YamlException ex)
                {
                    throw ex;
                }
            }

            return MachineConfigDictionaryConverter.Convert(configDictionary, looseMode);
        }

        protected static JObject ConfigModelToJObject<T>(T input)
        {
            var normalizedJson = ConfigModelJsonSerializer.Serialize(input);
            return JObject.Parse(normalizedJson);

        }

    }
}