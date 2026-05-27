using System.Linq;
using System.Management.Automation;
using System.Net.Sockets;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.Catlets;

[PublicAPI]
[Cmdlet(VerbsCommon.Get, "CatletIp", DefaultParameterSetName = "list")]
[OutputType(typeof(NetworkPortIp))]
public class GetCatletIpCommand : CatletCmdLet
{
    [Parameter(
        ParameterSetName = "get",
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true)]
    public string[] Id { get; set; }

    [Parameter(
        ParameterSetName = "list",
        Position = 0)]
    [ValidateNotNullOrEmpty]
    public string Name { get; set; }

    [Parameter(
        ParameterSetName = "list",
        ValueFromPipelineByPropertyName = true)]
    [ValidateNotNullOrEmpty]
    public string ProjectName { get; set; }

    [Parameter]
    public SwitchParameter InternalIp { get; set; }

    [Parameter]
    public string Network { get; set; }

    protected override void ProcessRecord()
    {
        if (Id != null)
        {
            foreach (var id in Id)
            {
                if (Stopping) break;
                if (TryGetById(id, GetSingleCatlet, "catlet", out var catlet))
                    WriteIp(catlet);
            }

            return;
        }

        var projectId = GetProjectId(ProjectName);
        WriteByNameOrId(
            Name,
            GetSingleCatlet,
            () => Factory.CreateCatletsClient().List(projectId: projectId),
            catlet => catlet.Name,
            "catlet",
            WriteIp);
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
