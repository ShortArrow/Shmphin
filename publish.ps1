#! pwsh

$targets = @{
  "win-x64" = "Shmphin.exe"
  "linux-x64" = "Shmphin"
  "linux-arm64" = "Shmphin"
  "linux-arm" = "Shmphin"
}

foreach ($key in $targets.Keys) {
  dotnet publish -c Release main -r $key
  mkdir -Force "./release/$key"
  Copy-Item "./main/bin/Release/net8.0/$key/publish/$($targets[$key])" "./release/$key/"
  Get-ChildItem "./release/$key/$($targets[$key])"
  Write-Host "Copied $($targets[$key]) to ./release/$key/"
}

