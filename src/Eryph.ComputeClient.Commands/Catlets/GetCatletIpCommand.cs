using System.Linq;
using System.Management.Automation;
using System.Net.Sockets;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.Catlets;

[PublicAPI]
[Cmdlet(VerbsCommon.Get, "CatletIp", DefaultParameterSetName = "get")]
[OutputType(typeof(NetworkPortIp), ParameterSetName = ["get"])]
public class GetCatletIpCommand : CatletCmdLet
{
    [Parameter(
        ParameterSetName = "get",
        Position = 0,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true)]

    public string[] Id { get; set; }

    [Parameter(
        ParameterSetName = "get",
        Mandatory = false)]
    public SwitchParameter InternalIp { get; set; }

    [Parameter(
        ParameterSetName = "get",
        Mandatory = false)]
    public string Network { get; set; }

    protected override void ProcessRecord()
    {
        if (Id != null)
        {
            foreach (var id in Id)
            {
                var catlet = GetSingleCatlet(id);

                WriteIp(catlet);
            }

            return;
        }

        foreach (var catlet in Factory.CreateCatletsClient().List())
        {
            if (Stopping) break;

            WriteIp(catlet);
        }
    }

    private void WriteIp(Catlet catlet)
    {
        var addresses = (catlet.Networks ?? [])
            .Where(n => string.IsNullOrEmpty(Network) || n.Name == Network)
            .SelectMany(n => InternalIp ? n.IpV4Addresses ?? [] : n.FloatingPort?.IpV4Addresses ?? [])
            .Select(ip => new NetworkPortIp
            {
                Id = catlet.Id,
                Name = catlet.Name,
                IpAddress = ip,
                AddressFamily = AddressFamily.InterNetwork

            })
            .ToList();

        WriteObject(addresses, true);
    }
}

public class NetworkPortIp
{
    public string Id { get; set; }
    
    public string Name { get; set; }
    
    public string IpAddress { get; set; }

    public AddressFamily AddressFamily { get; set; }
}
