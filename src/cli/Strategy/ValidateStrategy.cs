namespace Linterhub.Cli.Strategy
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using Microsoft.Framework.Configuration;
    using Linterhub.Cli.Runtime;
    using Linterhub.Engine;
    using Linterhub.Engine.Exceptions;

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
            var configurationBuilder = new ConfigurationBuilder().AddJsonFile(filePath);
            var configuration = configurationBuilder.Build();
            var config = new Configuration
            {
                Command = configuration["command"],
                CommandInfo = configuration["command_info"],
                Linterhub = configuration["linterhub"],
                Terminal = configuration["terminal"],
                TerminalCommand = configuration["terminalCommand"],
                ProjectConfig = configuration["projectConfig"]
            };

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