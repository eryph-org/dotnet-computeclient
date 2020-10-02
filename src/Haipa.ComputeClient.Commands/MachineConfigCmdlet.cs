using Haipa.ComputeClient.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Haipa.ComputeClient.Commands
{
    public class MachineConfigCmdlet : ComputeCmdLet
    {
        protected MachineConfig DeserializeConfigString(string configString)
        {

            if (configString.StartsWith("{") && configString.EndsWith("}"))
                return DeserializeConfigStringAsJson(configString);

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            return deserializer.Deserialize<MachineConfig>(configString);
        }

        private MachineConfig DeserializeConfigStringAsJson(string configString)
        {
            return Microsoft.Rest.Serialization.SafeJsonConvert.DeserializeObject<MachineConfig>(configString);

        }
    }
}