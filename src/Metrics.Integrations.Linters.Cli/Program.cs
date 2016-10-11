namespace Metrics.Integrations.Linters
{
    using System;
    using System.IO;
    using Newtonsoft.Json;
    using System.Linq;
    using System.Diagnostics;
    using Mono.Options;
    using Runtime;
    using Extensions;

    // TODO: Improve readability.
    internal class Program
    {
        internal static void Main(string[] args)
        {
            using (var log = new LogManager())
            {
                Run(args, log);
            }
        }

        internal static void Run(string[] args, LogManager log)
        {
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

            log.Trace("Run  :", runContext.Mode);
            switch (runContext.Mode)
            {
                case RunContext.ModeEnum.Analyze:
                    Analyze(config, runContext, record, log);
                    break;
                // TODO: Add more modes.
            } 
        }

        private static void Analyze(
            Configuration config,
            RunContext runContext,
            Registry.Record record,
            LogManager log)
        {
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

            var engine = new Engine(linterContext, log);
            CmdWrapper.RunResults run;
            try
            {
                run = engine.Run(linter, linterArgs);
                if (run.RunException != null)
                {
                    throw run.RunException;
                }
            }
            catch (Exception e)
            {
                log.Error("Runtime error:" + e.Message);
                return;
            }

            log.Trace("Exec status:", run.ExitCode);

            if (!File.Exists(linterContext.OutputFullPath))
            {
                log.Error("Linter output was not found:", linterContext.OutputFullPath);
                return;
            }

            string output;
            try
            {
                output = File.ReadAllText(linterContext.OutputFullPath);
            }
            catch (Exception e)
            {
                log.Error("Error reading linter output:" + e.Message);
                return;
            }
            finally
            {
                File.Delete(linterContext.OutputFullPath);
            }

            var logFolder = linterContext.GetLinterLogFolder();
            log.Trace("Log folder", logFolder);
            if (Directory.Exists(logFolder))
            {
                log.Trace("Write logs");
                try
                {
                    File.WriteAllText(linterContext.GetLinterLogFile("output"), run.Output?.ToString());
                    File.WriteAllText(linterContext.GetLinterLogFile("error"), run.Error?.ToString());
                    File.WriteAllText(linterContext.GetLinterLogFile("raw"), output);
                }
                catch (Exception e)
                {
                    log.Error("Error writing runtime logs:" + e.Message);
                    return;
                }
            }

            ILinterResult linterResult;
            try
            {
                linterResult = engine.Parse(linter, linterArgs, output);
                log.Trace("Linter results were parsed");
            }
            catch (Exception e)
            {
                log.Error("Error parsing results from linter:" + e.Message);
                return;
            }

            ILinterModel linterModel;
            try
            {
                linterModel = engine.Map(linter, linterResult);
                log.Trace("Linter results were mapped");
            }
            catch (Exception e)
            {
                log.Error("Error mapping results from linter:" + e.Message);
                return;
            }

            var mapString = JsonConvert.SerializeObject(linterModel);

            if (Directory.Exists(logFolder))
            {
                try
                {
                    File.WriteAllText(linterContext.GetLinterLogFile("map"), mapString);
                }
                catch (Exception e)
                {
                    log.Error("Error writing map logs:" + e.Message);
                    return;
                }

                if (linterModel != null)
                {
                    log.Trace("Save execution logs");
                    log.Save(linterContext.GetLinterLogFile("exec"));
                }
            }

            Console.WriteLine(mapString);
        }
    }
}
