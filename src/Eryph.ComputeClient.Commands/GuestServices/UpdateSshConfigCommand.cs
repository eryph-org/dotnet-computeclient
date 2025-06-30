using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.GuestServices;

[PublicAPI]
[Cmdlet(VerbsData.Update, "EryphSshConfig")]
public class UpdateSshConfigCommands : ComputeCmdLet
{
    protected override void EndProcessing()
    {
        var sshConfigPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".ssh",
            "config");

        var eryphSshConfigPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            ".eryph",
            "ssh",
            "config");

        var include = $"Include {eryphSshConfigPath}{Environment.NewLine}";
        if (!File.Exists(sshConfigPath))
        {
            File.WriteAllText(sshConfigPath, include);
        }
        else
        {
            var sshConfig = File.ReadAllText(sshConfigPath);
            if (!sshConfig.Contains(include))
            {
                File.WriteAllText(sshConfigPath, $"{include}{Environment.NewLine}{sshConfig}");
            }
        }

        var identityFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".ssh",
            "id_eryph");
        var catlets = Factory.CreateCatletsClient().List();
        StringBuilder builder = new StringBuilder();
        foreach (var catlet in catlets)
        {
            builder.AppendLine($"Host eryph-{catlet.Name}");
            builder.AppendLine($"    HostName {catlet.Name}");
            builder.AppendLine($"    User egs");
            builder.AppendLine($"    IdentityFile {identityFilePath}");
            builder.AppendLine($"    ProxyCommand  hvc nc -t vsock {catlet.VmId} 5002");
            builder.AppendLine("");
        }

        if (!Directory.Exists(Path.GetDirectoryName(eryphSshConfigPath)))
            Directory.CreateDirectory(Path.GetDirectoryName(eryphSshConfigPath)!);

        File.WriteAllText(eryphSshConfigPath, builder.ToString());

        base.EndProcessing();
    }
}
