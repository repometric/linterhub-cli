export DOTNET_RUNTIME_ID=osx.10.11-x64
dotnet restore
dotnet build
dotnet publish
cp -a bin/debug/netcoreapp1.0/osx.10.11-x64/publish/. bin/publish
#dotnet bin/publish/metrics.integrations.linters.cli.dll --config config.MacOS.json --linter htmlhint --project /Volumes/Repositories/Test
#dotnet bin/publish/metrics.integrations.linters.cli.dll --config config.MacOS.json --mode Linters
#dotnet bin/publish/metrics.integrations.linters.cli.dll --mode Generate --linter jslint --project /Volumes/Repositories/Test