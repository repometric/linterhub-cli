#!/bin/sh
TestPath=C:\Users\Andrey\Desktop\linterhub-tests

Current=$PWD
echo $Current
cd bin/dotnet
./cli --mode=analyze --linter=jshint --project=$TestPath\jquery

cd $Current