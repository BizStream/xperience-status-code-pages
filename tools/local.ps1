
dotnet clean
dotnet build

Remove-Item .\packages\* -Recurse -Force

dotnet pack .\src\Administration -o .\packages
dotnet pack .\src\AspNetCore -o .\packages

Get-ChildItem \NugetServer\BizStream.Kentico.Xperience.*.StatusCodePages -Recurse | Remove-Item -Recurse -Force -Confirm:$false

nuget init .\packages \NugetServer
