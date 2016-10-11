namespace Metrics.Integrations.Linters.Runtime
{
    using System.IO;
    using System.Linq;
    using Microsoft.Framework.Configuration;

    public static class Extensions
    {
        public const string LinterHubFolder = ".linterhub";
        public const string LogFolder = "logs";
        public const string ConfigFile = "config.json";
        public const string LogExtension = ".log";
        public const string LogFormat = "yyyy.MM.dd_HH.mm.ss";

        public static Configuration GetAppConfiguration(this RunContext self)
        {
            var configurationBuilder = new ConfigurationBuilder().AddJsonFile(self.Configuration);
            var configuration = configurationBuilder.Build();
            var config = new Configuration
            {
                Command = configuration["command"],
                Linterhub = configuration["linterhub"],
                Terminal = configuration["terminal"],
                TerminalCommand = configuration["terminalCommand"]
            };

            return config;
        }

        public static Registry.Record GetLinterRecord(this RunContext self)
        {
            return Registry.Get().SingleOrDefault(x => x.Name == self.Linter);
        }

        public static string GetLinterFolder(this LinterContext self)
        {
            return Path.Combine(self.RunContext.Project, LinterHubFolder, self.RunContext.Linter);
        }

        public static string GetLinterConfigFile(this LinterContext self)
        {
            return Path.Combine(self.GetLinterFolder(), ConfigFile);
        }

        public static string GetLinterLogFolder(this LinterContext self)
        {
            return Path.Combine(self.GetLinterFolder(), LogFolder);
        }

        public static string GetLinterLogFile(this LinterContext self, string type)
        {
            return Path.Combine(self.GetLinterLogFolder(), self.Stamp + "_" + type + LogExtension);
        }
    }
}