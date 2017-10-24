dotnet restore src/cli/cli.csproj && \
dotnet publish src/cli/cli.csproj --output ../../bin/$1/ -f netcoreapp2.0 -c release
#zip -r bin/linterhub-cli-$1.zip bin/$1 #&& \
#rm -R bin/$1