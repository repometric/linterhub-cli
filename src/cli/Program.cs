namespace Linterhub.Cli
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Newtonsoft.Json;
    using Runtime;
    using Strategy;
    using Engine;

    internal class Program
    {
        internal static void Main(string[] args)
        {
            using (var log = new LogManager())
            {
                Run(args, log);
            }
        }

        internal static Dictionary<RunMode, IStrategy> Strategies = new Dictionary<RunMode, IStrategy>
        {
            { RunMode.Catalog, new CatalogStrategy() },
            { RunMode.Generate, new GenerateStrategy() },
            { RunMode.Analyze, new AnalyzeStrategy() },
            { RunMode.Version, new VersionStrategy() },
            { RunMode.Activate, new ActivateStrategy() }
        };

        internal static void Run(string[] args, LogManager log)
        {
            log.Trace("Start  :", Process.GetCurrentProcess().ProcessName);
            log.Trace("Args   :", args);

            var optionsStrategy = new OptionsStrategy();
            var validateStrategy = new ValidateStrategy();
            var engine = new LinterEngine();
            Strategies.Add(RunMode.Help, optionsStrategy);
            var context = new RunContext();
            try
            {
                context = optionsStrategy.Parse(args);
                validateStrategy.Run(context, engine, log);
            }
            catch (Exception exception) 
            {
                log.Error(exception);
                return;
            }

            try
            {
                log.Trace("Mode   :", context.Mode);
                log.Trace("Config :", context.Config);
                log.Trace("Linter :", context.Linter);
                log.Trace("Project:", context.Project);

                //var linters = new List<string>();
                //if (context.Linter != null)
                //{
                //    linters.Add(context.Linter);
                //}
                //else
                //{
                //    var projectConfigPath = Path.Combine(context.Project, ".linterhub.json");
                //    if (File.Exists(projectConfigPath))
                //    {
                //        using (var fs = File.Open(projectConfigPath, FileMode.Open))
                //        {
                //            var projectConfig = fs.DeserializeAsJson<ExtConfig>();
                //            linters.AddRange(projectConfig.Linters.Select(x => x.Name).ToList());
                //        }
                //    }
                //    else
                //    {
                //        throw new LinterConfigNotFoundException(context.Project);
                //    }
                //}
/*
                var lintersResult = linters.Select(x => new
                {
                    Name = x,
                    Model = Strategies[context.Mode].Run(new RunContext
                    {
                        Mode = context.Mode,
                        Config = context.Config,
                        Linter = x,
                        Project = context.Project,
                        Configuration = context.Configuration,
                        Input = context.Input,
                        InputAwailable = context.InputAwailable
                    }, engine, log)
                }).ToArray();
*/
                var result = Strategies[context.Mode].Run(context, engine, log);
               // var result = strategy.Run(context, engine, log);

                if (result == null) return;
                var output = result is string 
                           ? result
                           : JsonConvert.SerializeObject(result);
                Console.Write(output);
            }
            catch (Exception exception)
            {
                log.Error(exception);
            }
        }
    }
}
