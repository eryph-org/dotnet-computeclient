using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using Eryph.ComputeClient.Commands.Catlets;
using Eryph.ComputeClient.Models;
using Eryph.ConfigModel.Json;
using JetBrains.Annotations;

namespace Eryph.ComputeClient.Commands.CatletSpecifications;

/// <summary>
/// Exports the fodder of a catlet specification version as cloud-init user-data,
/// e.g. to apply the configuration to a VM which is not managed by eryph.
/// The user-data contains secrets in plain text.
/// </summary>
[PublicAPI]
[Cmdlet(VerbsData.Export, "CatletSpecificationUserData", DefaultParameterSetName = "Parameter")]
[OutputType(typeof(string))]
public class ExportCatletSpecificationUserData : CatletConfigCmdlet
{
    [Parameter(
        ParameterSetName = "Parameter",
        Mandatory = true)]
    [ValidateNotNullOrEmpty]
    public string SpecificationId { get; set; }

    [Parameter(
        ParameterSetName = "Parameter",
        Mandatory = true)]
    [ValidateNotNullOrEmpty]
    public string SpecificationVersionId { get; set; }

    [Parameter(
        ParameterSetName = "Pipeline",
        Position = 0,
        Mandatory = true,
        ValueFromPipeline = true)]
    public CatletSpecificationVersion Version { get; set; }

    // Exports the specification's latest version.
    [Parameter(
        ParameterSetName = "Specification",
        Position = 0,
        Mandatory = true,
        ValueFromPipeline = true)]
    public CatletSpecification Specification { get; set; }

    [Parameter]
    public string Architecture { get; set; }

    [Parameter]
    public Hashtable Variables { get; set; }

    [Parameter]
    public SwitchParameter SkipVariablesPrompt { get; set; }

    // The values of the system variables catletId and vmId. A new ID is generated when
    // omitted. Pass the Hyper-V ID of the target VM as -VmId if the fodder uses it.
    [Parameter]
    public Guid? CatletId { get; set; }

    [Parameter]
    public Guid? VmId { get; set; }

    // Writes the user-data to this file instead of the pipeline.
    [Parameter]
    [ValidateNotNullOrEmpty]
    public string Path { get; set; }

    protected override void BeginProcessing()
    {
        base.BeginProcessing();
        RequireApiVersion(1, 3, "Export-CatletSpecificationUserData");
    }

    protected override void ProcessRecord()
    {
        if (Specification is not null && Specification.Latest is null)
        {
            WriteError(new ErrorRecord(
                new InvalidOperationException(
                    $"The catlet specification '{Specification.Name}' does not have any versions to export."),
                "NoVersions",
                ErrorCategory.ObjectNotFound,
                Specification));
            return;
        }

        var specificationId = Version?.SpecificationId ?? Specification?.Id ?? SpecificationId;
        var specificationVersionId = Version?.Id ?? Specification?.Latest?.Id ?? SpecificationVersionId;
        if (specificationId is null || specificationVersionId is null)
            return;

        var specificationsClient = Factory.CreateCatletSpecificationsClient();
        var specificationVersion = specificationsClient.GetVersion(specificationId, specificationVersionId);

        var variant = SelectVariant(specificationVersion.Value, Architecture);
        if (variant is null)
            return;

        var resolvedConfig = CatletConfigJsonSerializer.Deserialize(variant.BuiltConfig);
        var variables = resolvedConfig.Variables ?? [];

        if (!PopulateVariables(variables, Variables, SkipVariablesPrompt, false))
            return;

        var result = specificationsClient.ExportUserData(
            specificationId,
            specificationVersionId,
            new ExportCatletSpecificationUserDataRequestBody
            {
                Architecture = Architecture,
                Variables = variables.ToDictionary(v => v.Name, v => v.Value),
                CatletId = CatletId ?? Guid.NewGuid(),
                VmId = VmId ?? Guid.NewGuid(),
            });

        if (Path is null)
        {
            WriteObject(result.Value.UserData);
            return;
        }

        WriteUserData(GetUnresolvedProviderPathFromPSPath(Path), result.Value.UserData);
    }

    // The user-data contains secrets in plain text. On Unix, the file is restricted to
    // the current user, like e.g. ssh-keygen does it. On Windows, the file inherits the
    // ACL of its directory.
    private static void WriteUserData(string filePath, string userData)
    {
        var encoding = new UTF8Encoding(false);
#if NET8_0_OR_GREATER
        if (!OperatingSystem.IsWindows())
        {
            const UnixFileMode userOnly = UnixFileMode.UserRead | UnixFileMode.UserWrite;
            // The create mode only applies to new files. An existing file must be
            // restricted before the secrets are written to it.
            if (File.Exists(filePath))
                File.SetUnixFileMode(filePath, userOnly);

            using var stream = new FileStream(filePath, new FileStreamOptions
            {
                Mode = FileMode.Create,
                Access = FileAccess.Write,
                UnixCreateMode = userOnly,
            });
            using var writer = new StreamWriter(stream, encoding);
            writer.Write(userData);
            return;
        }
#endif
        File.WriteAllText(filePath, userData, encoding);
    }
}
