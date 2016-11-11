dotnet publish src/cli/project.json --output bin/osx.10.12-x64/ -c release -r osx.10.11-x64
dotnet publish src/cli/project.json --output bin/osx.10.11-x64/ -c release -r osx.10.11-x64
dotnet publish src/cli/project.json --output bin/debian.8-x64/ -c release -r debian.8-x64
dotnet publish src/cli/project.json --output bin/win8-x64/ -c release -r win8-x64
dotnet publish src/cli/project.json --output bin/win10-x64/ -c release -r win10-x64

docker build -t linterhub-cli .
mkdir bin/docker
docker save linterhub-cli > bin/docker/image.tar