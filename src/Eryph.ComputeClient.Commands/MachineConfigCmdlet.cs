using Eryph.ComputeClient.Models;
using Newtonsoft.Json.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Eryph.ComputeClient.Commands
{
    public class MachineConfigCmdlet : ComputeCmdLet
    {
        protected object DeserializeConfigString(string configString)
        {

            if (configString.StartsWith("{") && configString.EndsWith("}"))
                return DeserializeConfigStringAsJson(configString);

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            return deserializer.Deserialize<dynamic>(configString);
        }

        private object DeserializeConfigStringAsJson(string configString)
        {
            return Microsoft.Rest.Serialization.SafeJsonConvert.DeserializeObject<dynamic>(configString);

        }
    }
}