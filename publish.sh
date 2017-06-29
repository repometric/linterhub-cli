#!/bin/bash

# Nuget is not needed right now
# dotnet pack src/engine/project.json -c Release -o bin
Version='0.5.0'

sh script/package.native.sh dotnet $Version
#sh src/script/package.sh osx.10.10-x64 $Version
sh script/package.sh osx.10.11-x64 $Version
sh script/package.sh win8-x64 $Version
sh script/package.sh win10-x64 $Version
sh script/package.sh debian.8-x64 $Version
sh script/package.sh ubuntu.16.04-x64 $Version
sh script/package.sh win10-x86 $Version
exit 0

docker build -t linterhub-cli-dev -f Dev.Dockerfile . && \
docker save linterhub-cli-dev | gzip > bin/linterhub-cli-docker-dev-$Version.tar.gz

cp bin/linterhub-cli-debian.8-x64.tar.gz linterhub-cli-debian.8-x64.tar.gz && \
docker build -t linterhub-cli . && \
docker save linterhub-cli | gzip > bin/linterhub-cli-docker-$Version.tar.gz
rm linterhub-cli-debian.8-x64.tar.gz