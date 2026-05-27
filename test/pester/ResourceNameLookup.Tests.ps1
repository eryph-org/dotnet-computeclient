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

    It "<Cmd> takes -Name as positional parameter 0" -ForEach @(
        @{ Cmd = 'Get-Catlet' }
        @{ Cmd = 'Get-EryphProject' }
        @{ Cmd = 'Get-CatletSpecification' }
        @{ Cmd = 'Get-CatletGene' }
    ) {
        (Get-Command $Cmd).Parameters['Name'].ParameterSets['list'].Position | Should -Be 0
    }

    It "Get-CatletGene exposes an -Architecture filter in the 'list' set" {
        $p = (Get-Command Get-CatletGene).Parameters['Architecture']
        $p | Should -Not -BeNullOrEmpty
        $p.ParameterSets.Keys | Should -Contain 'list'
    }
}

Describe 'Get-EryphProject name-or-id (integration)' -Skip:(-not $eryphAvailable) {

    BeforeAll {
        # Project names are limited to 20 chars, so keep the prefix short.
        $prefix  = "pst$([guid]::NewGuid().ToString('N').Substring(0,6))"  # 9 chars
        $names   = "$prefix-a", "$prefix-b", "$prefix-c"
        foreach ($n in $names) { $null = New-EryphProject -Name $n }
        $created = @(Get-EryphProject -Name "$prefix-*")
    }

    AfterAll {
        foreach ($n in $names) {
            $p = Get-EryphProject -Name $n -ErrorAction SilentlyContinue
            if ($p) { Remove-EryphProject -Id $p.Id -Force -ErrorAction SilentlyContinue }
        }
    }

    It 'matches an exact name (named)' {
        (Get-EryphProject -Name "$prefix-a").Name | Should -Be "$prefix-a"
    }

    It 'matches an exact name positionally' {
        (Get-EryphProject "$prefix-a").Name | Should -Be "$prefix-a"
    }

    It 'resolves a positional GUID to an id lookup' {
        $one = $created | Select-Object -First 1
        (Get-EryphProject $one.Id).Id | Should -Be $one.Id
    }

    It 'matches a wildcard pattern' {
        (Get-EryphProject "$prefix-*" | Measure-Object).Count | Should -Be 3
    }

    It 'matches case-insensitively' {
        (Get-EryphProject "$prefix-A").Name | Should -Be "$prefix-a"
    }

    It 'errors on an exact name that does not exist (Get-Process convention)' {
        { Get-EryphProject -Name "$prefix-z" -ErrorAction Stop } | Should -Throw
    }

    It 'returns nothing for a wildcard with no match' {
        Get-EryphProject -Name "$prefix-z*" | Should -BeNullOrEmpty
    }
}

Describe 'Get-Catlet name-or-id (integration, read-only)' -Skip:(-not $eryphAvailable) {

    BeforeAll { $existing = @(Get-Catlet) }

    It 'returns only catlets whose name matches the pattern' {
        if ($existing.Count -eq 0) { Set-ItResult -Skipped -Because 'no catlets present'; return }
        $sample = $existing[0].Name
        Get-Catlet -Name $sample | ForEach-Object { $_.Name | Should -BeLike $sample }
    }

    It 'resolves a positional GUID to an id lookup' {
        if ($existing.Count -eq 0) { Set-ItResult -Skipped -Because 'no catlets present'; return }
        $one = $existing[0]
        (Get-Catlet $one.Id).Id | Should -Be $one.Id
    }

    It "wildcard '*' returns every catlet" {
        if ($existing.Count -eq 0) { Set-ItResult -Skipped -Because 'no catlets present'; return }
        (Get-Catlet -Name '*' | Measure-Object).Count | Should -Be $existing.Count
    }

    It 'errors on an exact name that does not exist' {
        { Get-Catlet -Name "zzz-$([guid]::NewGuid().ToString('N'))" -ErrorAction Stop } | Should -Throw
    }

    It 'returns nothing for a wildcard with no match' {
        Get-Catlet -Name "zzzz-no-such-catlet-*" | Should -BeNullOrEmpty
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

    It 'resolves a positional GUID to a gene record (GeneWithUsage)' {
        if ($genes.Count -eq 0) { Set-ItResult -Skipped -Because 'no genes present'; return }
        $one = $genes[0]
        (Get-CatletGene $one.Id).Id | Should -Be $one.Id
    }
}
