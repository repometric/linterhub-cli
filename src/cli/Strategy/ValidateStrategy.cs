namespace Linterhub.Cli.Strategy
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using Runtime;
    using Engine;
    using Engine.Exceptions;
    using Engine.Extensions;

    public class ValidateStrategy : IStrategy
    {
        public object Run(RunContext context, LinterFactory factory, LogManager log)
        {        
            context.Config = GetConfigurationPath(context.Config);    
            if (!File.Exists(context.Config))
            {
                throw new LinterEngineException("App configuration was not found: " + context.Config);
            }

            context.Project = context.GetProjectPath();
            context.File = context.File?.Replace("\\", "/");
            if (!Directory.Exists(context.Project))
            {
                throw new LinterEngineException("Project was not found: " + context.Project);
            }

            try
            {
                context.CliConfig = ParseConfiguration(context.Config);
            }
            catch (Exception exception)
            {
                throw new LinterEngineException("Error parsing app configuration", exception);
            }

            if (!string.IsNullOrEmpty(context.CliConfig.Linterhub.Path) && 
                !Directory.Exists(context.CliConfig.Linterhub.Path))
            {
                throw new LinterEngineException("Linterhub was not found: " + context.CliConfig.Linterhub);
            }

            return true;
        }

        private CliConfig ParseConfiguration(string filePath)
        {
            var content = File.ReadAllText(filePath);
            var config = content.DeserializeAsJson<CliConfig>();
            return config;
        }

        private string GetConfigurationPath(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                return filePath;
            }

            string path;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                path = "Windows.json";
            } else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                path = "MacOS.json";
            } else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                path = "Linux.json";
            } else
            {
                path = "Default.json";
            }

            path = "Config/" + path;
            return path;
        }
    }
}