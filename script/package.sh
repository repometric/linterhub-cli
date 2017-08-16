dotnet restore src/cli/project.json && \
dotnet publish src/cli/project.json --output bin/$1/ -c release -r $1 -f netstandard1.6.1 && \
zip -r bin/linterhub-cli-$1.zip bin/$1 && \
rm -R bin/$1
