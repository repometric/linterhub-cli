namespace Linterhub.Cli
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
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
            { RunMode.Activate, new ActivateStrategy() },
            { RunMode.Help, new OptionsStrategy() }
        };
        
        internal static void Run(string[] args, LogManager log)
        {
            log.Trace("Start  :", Process.GetCurrentProcess().ProcessName);
            log.Trace("Args   :", string.Join(" ", args));

            var optionsStrategy = Strategies[RunMode.Help] as OptionsStrategy;
            var validateStrategy = new ValidateStrategy();
            var engine = new LinterEngine();
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
                log.Trace("File: ", context.File);
                log.Trace("Dir: ", context.Dir);

                var result = Strategies[context.Mode].Run(context, engine, log);

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
