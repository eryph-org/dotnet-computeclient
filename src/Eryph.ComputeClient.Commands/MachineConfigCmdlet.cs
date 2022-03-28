using Eryph.ComputeClient.Models;
using Newtonsoft.Json.Linq;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Eryph.ComputeClient.Commands
{
    public class MachineConfigCmdlet : ComputeCmdLet
    {
        protected MachineConfig DeserializeConfigString(string configString)
        {

            if (configString.StartsWith("{") && configString.EndsWith("}"))
                return DeserializeConfigStringAsJson(configString);

            try
            {
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build();
                return deserializer.Deserialize<MachineConfig>(configString);
            }
            catch (YamlException ex)
            {
                throw ex;
            }
        }

        private MachineConfig DeserializeConfigStringAsJson(string configString)
        {
            return Microsoft.Rest.Serialization.SafeJsonConvert.DeserializeObject<MachineConfig>(configString);

        }
    }
}