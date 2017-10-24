dotnet restore src/cli/cli.csproj && \
dotnet publish src/cli/cli.csproj --output ../../bin/$1/ -c release -r $1 -f netstandard2.0
#zip -r bin/linterhub-cli-$1.zip bin/$1 && \
#rm -R bin/$1
