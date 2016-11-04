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
            log.Trace("Start:", Process.GetCurrentProcess().ProcessName);
            log.Trace("Args :", args);

            var optionsStrategy = new OptionsStrategy();
            var validateStrategy = new ValidateStrategy();
            var engine = new LinterEngine();
            Strategies.Add(RunMode.Help, optionsStrategy);
            RunContext runContext = new RunContext();
            try
            {
                runContext = optionsStrategy.Parse(args);
                validateStrategy.Run(runContext, engine);
            }
            catch (Exception exception) 
            {
                log.Error(exception.Message);
                runContext.Mode = RunMode.Help;
            }

            try
            {
                log.Trace("Mode :", runContext.Mode);
                var strategy = Strategies[runContext.Mode];
                var result = strategy.Run(runContext, engine);
                if (result != null)
                {
                    var jsonResult = JsonConvert.SerializeObject(result);
                    Console.WriteLine(jsonResult);
                }
            }
            catch (Exception exception)
            {
                log.Error(exception.Message);
            }
        }
    }
}
