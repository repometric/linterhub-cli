namespace Metrics.Integrations.Linters
{
    using System;
    using System.IO;
    using Newtonsoft.Json;
    using System.Linq;
    using System.Diagnostics;
    using Mono.Options;
    using Runtime;

    internal class Program
    {
        internal static void Main(string[] args)
        {
            var log = new LogManager();
            log.Trace("Start:", Process.GetCurrentProcess().ProcessName);
            log.Trace("Args :", args);

            var runContext = new RunContext();
            var options = new OptionSet 
            {
                { "c=|config=", "Path to configuration.", v => runContext.Configuration = v },
                { "l=|linter=", "The linter name.", v => runContext.Linter = v },
                { "p=|project=", "Path to project", v => runContext.Project = v },
                { "h|help",  "Show help.", v => runContext.Mode = RunContext.ModeEnum.Help },
            };

            try
            {
                var extra = options.Parse(args);
                if (extra.Any())
                {
                    throw new ArgumentException(string.Join(",", extra));
                }
            }
            catch (Exception e) 
            {
                log.Error("Error parsing arguments: " + e.Message);
                runContext.Mode = RunContext.ModeEnum.Help;
            }

            if (runContext.Mode == RunContext.ModeEnum.Help)
            {
                Console.WriteLine("Options:");
                options.WriteOptionDescriptions(Console.Out);
                return;
            }

            if (!Directory.Exists(runContext.Project))
            {
                log.Error("Project was not found:", runContext.Project);
                return;
            }

            if (!File.Exists(runContext.Configuration))
            {
                log.Error("App configuration was not found:", runContext.Configuration);
                return;
            }

            Configuration config;
            try
            {
                config = runContext.GetAppConfiguration();
            }
            catch (Exception e)
            {
                log.Error("Error reading app configuration: " + e.Message);
                return;
            }

            var record = runContext.GetLinterRecord();
            if (record == null)
            {
                log.Error("Linter was not found:", runContext.Linter);
            }

            var linterContext = new LinterContext(config, runContext);
            var linterConfigFile = linterContext.GetLinterConfigFile();
            if (!File.Exists(linterConfigFile))
            {
                log.Error("Linter configuration was not found:", linterConfigFile);
                return;
            }

            string linterConfiguration;
            try
            {
                linterConfiguration = File.ReadAllText(linterConfigFile);
            }
            catch (Exception e)
            {
                log.Error("Error reading linter configuration file:" + e.Message);
                return;
            }

            ILinter linter;
            ILinterArgs linterArgs;
            try
            {
                linter = (ILinter)Activator.CreateInstance(record.Linter);
                linterArgs = (ILinterArgs)JsonConvert.DeserializeObject(linterConfiguration, record.Args);
            }
            catch (Exception e)
            {
                log.Error("Error reading linter configuration:" + e.Message);
                return;
            }

            log.Trace("Run  :", runContext.Mode);
            ILinterModel result;
            try
            {
                var engine = new Engine(linterContext, log);
                result = engine.Run(linter, linterArgs);
                if (result == null)
                {
                    throw new ArgumentException("NULL result");
                }
            }
            catch (Exception e)
            {
                log.Error("Runtime error:" + e.Message);
                return;
            }

            Console.WriteLine(JsonConvert.SerializeObject(result));
        }
    }
}
