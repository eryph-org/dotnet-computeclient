using System.Text.RegularExpressions;
using Eryph.ConfigModel.Catlets;
using Eryph.ConfigModel.Json;
using Eryph.ConfigModel.Networks;
using Eryph.ConfigModel.Yaml;
using YamlDotNet.Core;

namespace Eryph.ComputeClient.Commands.Networks;

public class NetworkConfigCmdlet : NetworkCmdLet
{

    protected static ProjectNetworksConfig DeserializeConfigString(string configString)
    {
        configString = configString.Trim();
        configString = configString.Replace("\r\n", "\n");

        if (configString.StartsWith("{") && configString.EndsWith("}"))
            return ProjectNetworksConfigJsonSerializer.Deserialize(configString);


        return ProjectNetworksConfigYamlSerializer.Deserialize(configString);
    }
}
