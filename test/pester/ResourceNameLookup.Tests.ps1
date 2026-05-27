#Requires -Modules @{ ModuleName = 'Pester'; ModuleVersion = '5.0.0' }
<#
    Pester tests for name-based lookup (issue #69).

    Two layers:
      * "parameter surface" tests need no server and run anywhere (CI-safe).
      * "integration" tests exercise the real cmdlet -> HTTP -> filter path against
        a reachable eryph. They are skipped automatically when no eryph is available.

    The module is imported and the eryph connection is probed during Pester's
    discovery phase so that -Skip can react to it. Run via Invoke-PesterTests.ps1,
    which builds the module first.
#>

$ErrorActionPreference = 'Stop'

# --- Discovery-time setup: import the freshly built module and probe for eryph ---
$manifest = Get-ChildItem -Path (Join-Path $PSScriptRoot '..' '..' 'src/Eryph.ComputeClient.Commands/bin') `
    -Recurse -Filter 'Eryph.ComputeClient.psd1' -ErrorAction SilentlyContinue |
    Sort-Object LastWriteTime -Descending | Select-Object -First 1
if (-not $manifest) {
    throw "Built module not found. Build the Commands project first (Invoke-PesterTests.ps1 -Build)."
}
Import-Module $manifest.FullName -Force

$eryphAvailable = $false
try { $null = Get-EryphProject -ErrorAction Stop; $eryphAvailable = $true } catch { }

Describe 'Get-* -Name parameter surface (no server required)' {

    It "<Cmd> exposes a string -Name in the 'list' parameter set" -ForEach @(
        @{ Cmd = 'Get-Catlet' }
        @{ Cmd = 'Get-EryphProject' }
        @{ Cmd = 'Get-CatletSpecification' }
        @{ Cmd = 'Get-CatletGene' }
    ) {
        $p = (Get-Command $Cmd).Parameters['Name']
        $p | Should -Not -BeNullOrEmpty
        $p.ParameterType | Should -Be ([string])
        $p.ParameterSets.Keys | Should -Contain 'list'
    }

    It '-Name on Get-Catlet is not positional (must not clash with -Id)' {
        (Get-Command Get-Catlet).Parameters['Name'].ParameterSets['list'].Position |
            Should -BeLessThan 0
    }

    It "Get-CatletGene exposes an -Architecture filter in the 'list' set" {
        $p = (Get-Command Get-CatletGene).Parameters['Architecture']
        $p | Should -Not -BeNullOrEmpty
        $p.ParameterSets.Keys | Should -Contain 'list'
    }
}

Describe 'Get-EryphProject -Name (integration)' -Skip:(-not $eryphAvailable) {

    BeforeAll {
        # Project names are limited to 20 chars, so keep the prefix short.
        $prefix = "pst$([guid]::NewGuid().ToString('N').Substring(0,6))"  # 9 chars
        $names  = "$prefix-a", "$prefix-b", "$prefix-c"
        foreach ($n in $names) { $null = New-EryphProject -Name $n }
    }

    AfterAll {
        foreach ($n in $names) {
            $p = Get-EryphProject -Name $n -ErrorAction SilentlyContinue
            if ($p) { Remove-EryphProject -Id $p.Id -Force -ErrorAction SilentlyContinue }
        }
    }

    It 'matches an exact name' {
        (Get-EryphProject -Name "$prefix-a").Name | Should -Be "$prefix-a"
    }

    It 'matches a wildcard pattern' {
        (Get-EryphProject -Name "$prefix-*" | Measure-Object).Count | Should -Be 3
    }

    It 'matches case-insensitively' {
        (Get-EryphProject -Name "$prefix-A").Name | Should -Be "$prefix-a"
    }

    It 'returns nothing when no project matches' {
        Get-EryphProject -Name "$prefix-z" | Should -BeNullOrEmpty
    }
}

Describe 'Get-Catlet -Name (integration, read-only)' -Skip:(-not $eryphAvailable) {

    BeforeAll { $existing = @(Get-Catlet) }

    It 'returns only catlets whose name matches the pattern' {
        if ($existing.Count -eq 0) { Set-ItResult -Skipped -Because 'no catlets present'; return }
        $sample = $existing[0].Name
        Get-Catlet -Name $sample | ForEach-Object { $_.Name | Should -BeLike $sample }
    }

    It "wildcard '*' returns every catlet" {
        if ($existing.Count -eq 0) { Set-ItResult -Skipped -Because 'no catlets present'; return }
        (Get-Catlet -Name '*' | Measure-Object).Count | Should -Be $existing.Count
    }

    It 'returns nothing for a non-existent name' {
        Get-Catlet -Name "zzz-$([guid]::NewGuid().ToString('N'))" | Should -BeNullOrEmpty
    }
}

Describe 'Get-CatletGene -Name / -Architecture (integration, read-only)' -Skip:(-not $eryphAvailable) {

    BeforeAll { $genes = @(Get-CatletGene) }

    It 'filters genes by name pattern' {
        if ($genes.Count -eq 0) { Set-ItResult -Skipped -Because 'no genes present'; return }
        $sample = $genes[0].Name
        Get-CatletGene -Name $sample | ForEach-Object { $_.Name | Should -BeLike $sample }
    }

    It 'filters genes by architecture' {
        if ($genes.Count -eq 0) { Set-ItResult -Skipped -Because 'no genes present'; return }
        $arch = $genes[0].Architecture
        Get-CatletGene -Architecture $arch | ForEach-Object { $_.Architecture | Should -Be $arch }
    }
}
