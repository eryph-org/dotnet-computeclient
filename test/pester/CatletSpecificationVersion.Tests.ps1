#Requires -Modules @{ ModuleName = 'Pester'; ModuleVersion = '5.0.0' }
<#
    Pester tests for Get-CatletSpecificationVersion.

    Server-free "parameter surface" tests guarding two fixes:
      * Issue 1: 'Get-CatletSpecification | Get-CatletSpecificationVersion' must bind the
        piped specification's 'Id' to -SpecificationId (a CatletSpecification exposes its id
        as 'Id', not 'SpecificationId'). The version's own id parameter is -VersionId.
      * Issue 2: -Config emits the version's configuration content as a string.

    The module is imported during Pester's discovery phase. Run via Invoke-PesterTests.ps1,
    which builds the module first.
#>

$ErrorActionPreference = 'Stop'

# --- Discovery-time setup: import the freshly built module ---
$configuration = if ($env:ERYPH_TEST_CONFIGURATION) { $env:ERYPH_TEST_CONFIGURATION } else { 'Debug' }
$binDir = Join-Path $PSScriptRoot '..' '..' "src/Eryph.ComputeClient.Commands/bin/$configuration"
$manifest = Get-ChildItem -Path $binDir -Recurse -Filter 'Eryph.ComputeClient.psd1' -ErrorAction SilentlyContinue |
    Sort-Object LastWriteTime -Descending | Select-Object -First 1
if (-not $manifest) {
    throw "Built module not found under '$binDir'. Build the Commands project first (Invoke-PesterTests.ps1 -Build)."
}
Import-Module $manifest.FullName -Force

Describe 'Get-CatletSpecificationVersion parameter surface (no server required)' {

    It 'binds the parent specification id via -SpecificationId (aliased to Id so a piped CatletSpecification binds)' {
        $p = (Get-Command Get-CatletSpecificationVersion).Parameters['SpecificationId']
        $p                                                       | Should -Not -BeNullOrEmpty
        $p.ParameterType                                         | Should -Be ([string])
        $p.Aliases                                               | Should -Contain 'Id'
        $p.ParameterSets['list'].ValueFromPipelineByPropertyName | Should -BeTrue
    }

    It 'exposes the version id as -VersionId (not -Id, which would clash with the spec id)' {
        $params = (Get-Command Get-CatletSpecificationVersion).Parameters
        $params.ContainsKey('VersionId')          | Should -BeTrue
        $params['VersionId'].ParameterType        | Should -Be ([string[]])
        # The old -Id parameter must be gone; 'Id' is now only an alias of -SpecificationId.
        $params['SpecificationId'].Aliases        | Should -Contain 'Id'
    }

    It 'exposes a -Config switch that returns the configuration content as a string' {
        $cmd = Get-Command Get-CatletSpecificationVersion
        $cmd.Parameters['Config'].ParameterType   | Should -Be ([switch])
        # -Config lives in its own output-type set yielding a string (mirrors Get-Catlet -Config).
        ($cmd.OutputType | ForEach-Object { $_.Type }) | Should -Contain ([string])
    }

    It '-Config does not require -VersionId (defaults to the latest version) but requires it for the full version object' {
        $versionId = (Get-Command Get-CatletSpecificationVersion).Parameters['VersionId']
        # getconfig: optional -> falls back to the specification's latest version.
        $versionId.ParameterSets['getconfig'].IsMandatory | Should -BeFalse
        # get: mandatory -> a specific full version object must be addressed explicitly.
        $versionId.ParameterSets['get'].IsMandatory       | Should -BeTrue
    }
}

Describe 'Deploy-Catlet deployment input (no server required)' {

    It 'accepts a piped CatletSpecification (deploy its latest version)' {
        $p = (Get-Command Submit-CatletDeployment).Parameters['Specification']
        $p                                | Should -Not -BeNullOrEmpty
        $p.ParameterType.Name             | Should -Be 'CatletSpecification'
        $p.ParameterSets.Values.ValueFromPipeline | Should -Contain $true
    }

    It 'still accepts a piped full CatletSpecificationVersion' {
        $p = (Get-Command Submit-CatletDeployment).Parameters['Version']
        $p.ParameterType.Name             | Should -Be 'CatletSpecificationVersion'
        $p.ParameterSets.Values.ValueFromPipeline | Should -Contain $true
    }

    It 'still accepts explicit -SpecificationId / -SpecificationVersionId' {
        $params = (Get-Command Submit-CatletDeployment).Parameters
        $params['SpecificationId']        | Should -Not -BeNullOrEmpty
        $params['SpecificationVersionId'] | Should -Not -BeNullOrEmpty
    }
}
