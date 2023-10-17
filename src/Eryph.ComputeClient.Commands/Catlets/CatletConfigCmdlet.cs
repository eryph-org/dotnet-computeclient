using Eryph.ConfigModel.Json;
using Eryph.ConfigModel.Catlets;
using Eryph.ConfigModel.Yaml;
using YamlDotNet.Core;
using static Org.BouncyCastle.Math.EC.ECCurve;
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

    }
}