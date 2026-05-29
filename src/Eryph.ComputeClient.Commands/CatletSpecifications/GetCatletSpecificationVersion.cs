using System.Management.Automation;
using Eryph.ComputeClient.Models;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.CatletSpecifications;

[PublicAPI]
[Cmdlet(VerbsCommon.Get, "CatletSpecificationVersion", DefaultParameterSetName = "list")]
[OutputType(typeof(CatletSpecificationVersionInfo), ParameterSetName = ["list"])]
[OutputType(typeof(CatletSpecificationVersion), ParameterSetName = ["get"])]
[OutputType(typeof(string), ParameterSetName = ["getconfig"])]
public class GetCatletSpecificationVersion : CatletSpecificationCmdlet
{
    // The id of the parent catlet specification. Aliased to 'Id' and bound from
    // the pipeline by property name so that 'Get-CatletSpecification | Get-CatletSpecificationVersion'
    // works: a CatletSpecification exposes its id as 'Id', not 'SpecificationId'.
    [Parameter(
        ParameterSetName = "list",
        Mandatory = true,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true)]
    [Parameter(
        ParameterSetName = "get",
        Mandatory = true,
        ValueFromPipelineByPropertyName = true)]
    [Parameter(
        ParameterSetName = "getconfig",
        Mandatory = true,
        ValueFromPipelineByPropertyName = true)]
    [Alias("Id")]
    [ValidateNotNullOrEmpty]
    public string SpecificationId { get; set; }

    // The version is mandatory when retrieving the full version object, but optional
    // for -Config: without it, the configuration of the specification's latest version
    // is returned.
    [Parameter(
        ParameterSetName = "get",
        Position = 0,
        Mandatory = true)]
    [Parameter(
        ParameterSetName = "getconfig",
        Position = 0)]
    [Alias("SpecificationVersionId")]
    [ValidateNotNullOrEmpty]
    public string[] VersionId { get; set; }

    [Parameter(
        ParameterSetName = "getconfig",
        Mandatory = true)]
    public SwitchParameter Config { get; set; }

    protected override void ProcessRecord()
    {
        var specificationsClient = Factory.CreateCatletSpecificationsClient();

        if (Config.IsPresent)
        {
            foreach (var versionId in ResolveVersionIds(specificationsClient))
            {
                if (Stopping) break;

                var version = specificationsClient.GetVersion(SpecificationId, versionId).Value;
                WriteObject(version.Configuration.Content);
            }

            return;
        }

        if (VersionId is not null)
        {
            foreach (var versionId in VersionId)
            {
                if (Stopping) break;

                WriteObject(specificationsClient.GetVersion(SpecificationId, versionId).Value);
            }

            return;
        }

        var specificationVersions = specificationsClient.ListVersions(specificationId: SpecificationId);
        foreach (var specificationVersion in specificationVersions)
        {
            if (Stopping) break;

            WriteObject(specificationVersion, true);
        }
    }

    // The explicit -VersionId values, or the latest version of the specification when
    // none were given.
    private string[] ResolveVersionIds(CatletSpecificationsClient specificationsClient)
    {
        if (VersionId is not null)
            return VersionId;

        var specification = GetSingleCatletSpecification(SpecificationId);
        if (specification.Latest is null)
        {
            WriteError(new ErrorRecord(
                new ItemNotFoundException(
                    $"The catlet specification '{SpecificationId}' does not have any versions."),
                "NoVersions",
                ErrorCategory.ObjectNotFound,
                SpecificationId));
            return [];
        }

        return [specification.Latest.Id];
    }
}
