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
# Scope the search to the build configuration the runner selected (default Debug)
# so we import the matching build rather than just the newest one on disk.
$configuration = if ($env:ERYPH_TEST_CONFIGURATION) { $env:ERYPH_TEST_CONFIGURATION } else { 'Debug' }
$binDir = Join-Path $PSScriptRoot '..' '..' "src/Eryph.ComputeClient.Commands/bin/$configuration"
$manifest = Get-ChildItem -Path $binDir -Recurse -Filter 'Eryph.ComputeClient.psd1' -ErrorAction SilentlyContinue |
    Sort-Object LastWriteTime -Descending | Select-Object -First 1
if (-not $manifest) {
    throw "Built module not found under '$binDir'. Build the Commands project first (Invoke-PesterTests.ps1 -Build)."
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
        @{ Cmd = 'Get-CatletIp' }
        @{ Cmd = 'Get-VNetwork' }
        @{ Cmd = 'Get-CatletDisk' }
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
        @{ Cmd = 'Get-CatletIp' }
        @{ Cmd = 'Get-VNetwork' }
        @{ Cmd = 'Get-CatletDisk' }
    ) {
        (Get-Command $Cmd).Parameters['Name'].ParameterSets['list'].Position | Should -Be 0
    }

    It "Get-CatletGene exposes an -Architecture filter in the 'list' set" {
        $p = (Get-Command Get-CatletGene).Parameters['Architecture']
        $p | Should -Not -BeNullOrEmpty
        $p.ParameterSets.Keys | Should -Contain 'list'
    }

    It "<Cmd> exposes an -Environment filter in the 'list' set" -ForEach @(
        @{ Cmd = 'Get-VNetwork' }
        @{ Cmd = 'Get-CatletDisk' }
    ) {
        $p = (Get-Command $Cmd).Parameters['Environment']
        $p | Should -Not -BeNullOrEmpty
        $p.ParameterSets.Keys | Should -Contain 'list'
    }

    It "<Cmd> exposes an optional -ProjectName filter in the 'list' set" -ForEach @(
        @{ Cmd = 'Get-Catlet' }
        @{ Cmd = 'Get-CatletDisk' }
        @{ Cmd = 'Get-CatletIp' }
        @{ Cmd = 'Get-VNetwork' }
        @{ Cmd = 'Get-CatletSpecification' }
    ) {
        $p = (Get-Command $Cmd).Parameters['ProjectName']
        $p | Should -Not -BeNullOrEmpty
        $p.ParameterSets['list'].IsMandatory | Should -BeFalse
    }
}

Describe 'Action cmdlets accept name-or-id (parameter surface)' {

    It "<Cmd> exposes a 'Name' alias on -Id" -ForEach @(
        @{ Cmd = 'Start-Catlet' }
        @{ Cmd = 'Stop-Catlet' }
        @{ Cmd = 'Remove-Catlet' }
        @{ Cmd = 'Update-Catlet' }
        @{ Cmd = 'Remove-CatletDisk' }
        @{ Cmd = 'Remove-CatletSpecification' }
        @{ Cmd = 'Update-CatletSpecification' }
        @{ Cmd = 'Remove-EryphProject' }
    ) {
        (Get-Command $Cmd).Parameters['Id'].Aliases | Should -Contain 'Name'
    }

    It "<Cmd> takes its id/name target as positional parameter 0" -ForEach @(
        @{ Cmd = 'Start-Catlet' }
        @{ Cmd = 'Stop-Catlet' }
        @{ Cmd = 'Remove-Catlet' }
        @{ Cmd = 'Update-Catlet' }
        @{ Cmd = 'Remove-CatletDisk' }
        @{ Cmd = 'Remove-CatletSpecification' }
        @{ Cmd = 'Remove-EryphProject' }
    ) {
        $positions = (Get-Command $Cmd).Parameters['Id'].ParameterSets.Values.Position
        $positions | Should -Contain 0
    }

    It "<Cmd> exposes a -ProjectName parameter to scope name resolution" -ForEach @(
        @{ Cmd = 'Start-Catlet' }
        @{ Cmd = 'Stop-Catlet' }
        @{ Cmd = 'Remove-Catlet' }
        @{ Cmd = 'Update-Catlet' }
        @{ Cmd = 'Remove-CatletDisk' }
        @{ Cmd = 'Remove-CatletSpecification' }
        @{ Cmd = 'Update-CatletSpecification' }
    ) {
        (Get-Command $Cmd).Parameters['ProjectName'] | Should -Not -BeNullOrEmpty
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

    It 'returns only catlets whose name matches an exact name' {
        if ($existing.Count -eq 0) { Set-ItResult -Skipped -Because 'no catlets present'; return }
        $sample = $existing[0].Name
        $result = @(Get-Catlet -Name $sample)
        $result | Should -Not -BeNullOrEmpty
        $result | ForEach-Object { $_.Name | Should -BeExactly $sample }
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

    It '-Config (without -Id) returns YAML config strings, not catlet objects' {
        if ($existing.Count -eq 0) { Set-ItResult -Skipped -Because 'no catlets present'; return }
        $config = @(Get-Catlet -Config)
        $config | Should -Not -BeNullOrEmpty
        $config[0] | Should -BeOfType [string]
    }
}

Describe 'Get-CatletIp name-or-id (integration, read-only)' -Skip:(-not $eryphAvailable) {

    BeforeAll { $withIp = @(Get-Catlet | Where-Object { (Get-CatletIp -Name $_.Name) }) }

    It 'filters by catlet name and returns IPs for that catlet' {
        if ($withIp.Count -eq 0) { Set-ItResult -Skipped -Because 'no catlet has an IP'; return }
        $sample = $withIp[0].Name
        Get-CatletIp -Name $sample | ForEach-Object { $_.Name | Should -BeExactly $sample }
    }

    It 'resolves a positional GUID to the catlet id' {
        if ($withIp.Count -eq 0) { Set-ItResult -Skipped -Because 'no catlet has an IP'; return }
        $one = $withIp[0]
        Get-CatletIp $one.Id | ForEach-Object { $_.Id | Should -Be $one.Id }
    }

    It 'returns nothing for a catlet name that matches nothing' {
        Get-CatletIp -Name 'zzzz-no-such-catlet-*' | Should -BeNullOrEmpty
    }
}

Describe 'Action cmdlets name resolution (integration, non-destructive)' -Skip:(-not $eryphAvailable) {

    It 'requires -ProjectName when the target is given by name (no cross-project fan-out)' {
        { Start-Catlet -Name 'some-catlet' -ErrorAction Stop } |
            Should -Throw -ExpectedMessage '*ProjectName is required*'
    }

    It 'does not require -ProjectName when the target is given by id' {
        # A random GUID is treated as an id; it should fail with not-found, NOT with the
        # ProjectName-required error.
        { Start-Catlet -Id ([guid]::NewGuid().ToString()) -ErrorAction Stop } |
            Should -Throw -ExpectedMessage '*Cannot find*'
    }

    It 'Start-Catlet errors on a non-existent exact name within a project (nothing is started)' {
        { Start-Catlet -Name "zzz-$([guid]::NewGuid().ToString('N'))" -ProjectName 'default' -ErrorAction Stop } |
            Should -Throw
    }

    It 'Remove-Catlet with a non-matching wildcard in a project does nothing and does not error' {
        { Remove-Catlet -Name 'zzzz-no-such-catlet-*' -ProjectName 'default' -Force -ErrorAction Stop } |
            Should -Not -Throw
    }
}

Describe 'Get-VNetwork name-or-id / -Environment (integration, read-only)' -Skip:(-not $eryphAvailable) {

    BeforeAll { $networks = @(Get-VNetwork) }

    It 'filters networks by name' {
        if ($networks.Count -eq 0) { Set-ItResult -Skipped -Because 'no networks present'; return }
        $sample = $networks[0].Name
        Get-VNetwork -Name $sample | ForEach-Object { $_.Name | Should -BeExactly $sample }
    }

    It 'filters networks by environment (and excludes other environments)' {
        if ($networks.Count -eq 0) { Set-ItResult -Skipped -Because 'no networks present'; return }
        $environment = $networks[0].Environment
        $filtered = @(Get-VNetwork -Environment $environment)
        $filtered | ForEach-Object { $_.Environment | Should -Be $environment }
        $expected = @($networks | Where-Object Environment -EQ $environment).Count
        $filtered.Count | Should -Be $expected
    }

    It '-Config returns the project network configuration as a string' {
        $config = Get-VNetwork -Config -ProjectName 'default'
        $config | Should -Not -BeNullOrEmpty
        $config | Should -BeOfType [string]
    }

    It 'resolves a positional GUID to an id lookup' {
        if ($networks.Count -eq 0) { Set-ItResult -Skipped -Because 'no networks present'; return }
        $one = $networks[0]
        (Get-VNetwork $one.Id).Id | Should -Be $one.Id
    }
}

Describe 'Get-CatletDisk name-or-id / -Environment (integration, read-only)' -Skip:(-not $eryphAvailable) {

    BeforeAll { $disks = @(Get-CatletDisk) }

    It 'filters disks by name' {
        if ($disks.Count -eq 0) { Set-ItResult -Skipped -Because 'no disks present'; return }
        $sample = $disks[0].Name
        Get-CatletDisk -Name $sample | ForEach-Object { $_.Name | Should -BeExactly $sample }
    }

    It 'filters disks by environment (and excludes other environments)' {
        if ($disks.Count -eq 0) { Set-ItResult -Skipped -Because 'no disks present'; return }
        $environment = $disks[0].Environment
        $filtered = @(Get-CatletDisk -Environment $environment)
        $filtered | ForEach-Object { $_.Environment | Should -Be $environment }
        $expected = @($disks | Where-Object Environment -EQ $environment).Count
        $filtered.Count | Should -Be $expected
    }

    It 'resolves a positional GUID to an id lookup' {
        if ($disks.Count -eq 0) { Set-ItResult -Skipped -Because 'no disks present'; return }
        $one = $disks[0]
        (Get-CatletDisk $one.Id).Id | Should -Be $one.Id
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

    It 'filters genes by name and architecture together' {
        if ($genes.Count -eq 0) { Set-ItResult -Skipped -Because 'no genes present'; return }
        $g = $genes[0]
        Get-CatletGene -Name $g.Name -Architecture $g.Architecture | ForEach-Object {
            $_.Name | Should -BeLike $g.Name
            $_.Architecture | Should -Be $g.Architecture
        }
    }

    It 'resolves a positional GUID to a gene record (GeneWithUsage)' {
        if ($genes.Count -eq 0) { Set-ItResult -Skipped -Because 'no genes present'; return }
        $one = $genes[0]
        (Get-CatletGene $one.Id).Id | Should -Be $one.Id
    }
}
