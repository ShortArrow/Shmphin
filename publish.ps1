#! pwsh

dotnet publish main -c Release

$name = "Shmphin.exe"

mkdir -Force "./release/"
Copy-Item "./main/bin/Release/net8.0/publish/$name" "./release/"
Get-ChildItem "./release/$name"
