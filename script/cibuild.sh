#!/bin/bash

# Nuget is not needed right now
# dotnet pack src/engine/project.json -c Release -o bin
Version='0.5.0'

sh script/package.native.sh dotnet $Version
#sh script/package.sh osx.10.11-x64 $Version
#sh script/package.sh win8-x64 $Version
#sh script/package.sh win10-x64 $Version
#sh script/package.sh debian.8-x64 $Version
#sh script/package.sh ubuntu.14.04-x64 $Version
#sh script/package.sh ubuntu.16.04-x64 $Version