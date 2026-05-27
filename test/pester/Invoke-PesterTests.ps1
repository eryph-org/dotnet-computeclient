#Requires -Version 7.4
<#
    .SYNOPSIS
        Runs the Pester tests for the Eryph.ComputeClient module.
    .DESCRIPTION
        Optionally (re)builds the PowerShell module, then runs the Pester tests in
        this folder. The "parameter surface" tests run without a server; the
        integration tests run only when a local eryph is reachable and are skipped
        otherwise.
    .EXAMPLE
        ./Invoke-PesterTests.ps1 -Build
        Builds the module and runs all tests.
#>
[CmdletBinding()]
param(
    # Build the Commands project before running the tests.
    [switch] $Build,
    # Build configuration to use when -Build is set / when locating the module.
    [string] $Configuration = 'Debug'
)

$ErrorActionPreference = 'Stop'
$repoRoot = Resolve-Path (Join-Path $PSScriptRoot '..' '..')

if ($Build) {
    $csproj = Join-Path $repoRoot 'src/Eryph.ComputeClient.Commands/Eryph.ComputeClient.Commands.csproj'
    Write-Host "Building $csproj ($Configuration)..." -ForegroundColor Cyan
    dotnet build $csproj -c $Configuration
    if ($LASTEXITCODE -ne 0) { throw "Build failed with exit code $LASTEXITCODE." }
}

# Pester v5 is required.
$pester = Get-Module Pester -ListAvailable | Where-Object Version -ge ([version]'5.0.0') |
    Sort-Object Version -Descending | Select-Object -First 1
if (-not $pester) {
    throw "Pester 5+ is required. Install it with: Install-Module Pester -MinimumVersion 5.0.0 -Scope CurrentUser"
}
Import-Module $pester.Path -Force

$config = New-PesterConfiguration
$config.Run.Path = $PSScriptRoot
$config.Output.Verbosity = 'Detailed'
$config.Run.Exit = $true

Invoke-Pester -Configuration $config
