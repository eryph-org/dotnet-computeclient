#Requires -Version 7.4
<#
    .SYNOPSIS
        Generates the REST client locally
    .DESCRIPTION
        This script generates the REST client locally.
#>
[CmdletBinding()]
param()

$PSNativeCommandUseErrorActionPreference = $true
$ErrorActionPreference = 'Stop'

$settings = Get-Content -Raw -Path "$PSScriptRoot/config.json" | ConvertFrom-Json
$tag = $settings.tag
$spec = $settings.spec

npm exec --package="autorest" -- `
    autorest `
    --version="3.10.2" `
    --use="@autorest/csharp@3.0.0-beta.20240527.2" `
    --use="@autorest/modelerfour@4.27.0" `
    "$PSScriptRoot/../../eryph-api-spec/specification/$spec" `
    --tag=$tag `
    --csharp-src-folder="$PSScriptRoot/.." `
    --v3 `
    --csharp `
    --skip-csproj=true `
    --generate-sample-project=false `
    --verbose
