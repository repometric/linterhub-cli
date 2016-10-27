FROM microsoft/dotnet
MAINTAINER Repometric <docker@repometric.com>
WORKDIR /parser
COPY . .
RUN dotnet restore
WORKDIR /parser/src/Metrics.Integrations.Linters.Cli
RUN dotnet publish -c Release -o out
ENTRYPOINT ["dotnet", "/parser/src/Metrics.Integrations.Linters.Cli/out/Metrics.Integrations.Linters.Cli.dll"]