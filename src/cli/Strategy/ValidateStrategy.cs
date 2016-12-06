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
        public object Run(RunContext context, LinterEngine engine, LogManager log)
        {        
            context.Config = GetConfigurationPath(context.Config);    
            if (!File.Exists(context.Config))
            {
                throw new LinterEngineException("App configuration was not found: " + context.Config);
            }

            context.Project = GetProjectPath(context.Project);
            if (!Directory.Exists(context.Project))
            {
                throw new LinterEngineException("Project was not found: " + context.Project);
            }

            try
            {
                context.Configuration = ParseConfiguration(context.Config);
            }
            catch (Exception exception)
            {
                throw new LinterEngineException("Error parsing app configuration", exception);
            }

            if (!string.IsNullOrEmpty(context.Configuration.Linterhub) && 
                !Directory.Exists(context.Configuration.Linterhub))
            {
                throw new LinterEngineException("Linterhub was not found: " + context.Configuration.Linterhub);
            }

            return true;
        }

        private Configuration ParseConfiguration(string filePath)
        {
            var content = File.ReadAllText(filePath);
            var config = content.DeserializeAsJson<Configuration>();
            return config;
        }

        private string GetConfigurationPath(string filePath)
        {
            string path;
            if (string.IsNullOrEmpty(filePath))
            {
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

            return filePath;
        }

        private string GetProjectPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return Directory.GetCurrentDirectory();
            }

            return path;
        }
    }
}