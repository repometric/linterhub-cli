namespace Linterhub.Cli
{
    using System;
    using System.IO;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;
    using System.Diagnostics;
    using Mono.Options;
    using Runtime;
    using Strategy;
    using Linterhub.Engine;
    using Linterhub.Engine.Extensions;
    using Linterhub.Engine.Linters;
    using System.Runtime.InteropServices;

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
            { RunMode.Analyze, new AnalyzeStrategy() }
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

                var strategy = Strategies[context.Mode];
                var result = strategy.Run(context, engine, log);
                if (result != null)
                {
                    var jsonResult = JsonConvert.SerializeObject(result);
                    Console.WriteLine(jsonResult);
                }
            }
            catch (Exception exception)
            {
                log.Error(exception);
            }
        }
    }
}
