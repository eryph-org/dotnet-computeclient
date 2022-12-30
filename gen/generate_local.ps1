Push-Location $PSScriptRoot
$settings = Get-Content -Raw -Path "config.json" | ConvertFrom-Json
cd ..
$location = Get-Location
$tag = $settings.tag
$spec = $settings.spec

autorest  ..\eryph-api-spec\specification\$spec --tag=$tag --csharp-src-folder=$location --use=..\autorest.csharp\artifacts\bin\AutoRest.CSharp\Debug\netcoreapp3.1 --v3 --csharp