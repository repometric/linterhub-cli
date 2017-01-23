dotnet restore src/cli/project.json && \
dotnet publish src/cli/project.json --output bin/$1/ -f netcoreapp1.1 -c release && \
#tar -czf bin/linterhub-cli-$1-$2.tar.gz bin/$1 && \
zip -r bin/linterhub-cli-$1-$2.zip bin/$1 && \
rm -R bin/$1
