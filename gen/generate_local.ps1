Push-Location $PSScriptRoot
$settings = Get-Content -Raw -Path "config.json" | ConvertFrom-Json
cd ..
$location = Get-Location
$tag = $settings.tag
$spec = $settings.spec

autorest  ..\eryph-api-spec\specification\$spec --tag=$tag --csharp-src-folder=$location --v3 --csharp --verbose