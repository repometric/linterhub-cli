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
                //string[] test1 =
                //{
                //    @"--mode=Analyze",
                //    @"--project=D:\work\repometric\Lints_Test"
                //};
                //Run(test1, log);

                //string[] test2 =
                //{
                //    @"--mode=Analyze",
                //    @"--project=D:\work\repometric\Lints_Test",
                //    @"--file=D:\work\repometric\Lints_Test\Test_Js\easing.js",
                //};
                //Run(test2, log);

                //string[] test3 = {
                //    @"--mode=Analyze",
                //    @"--project=D:\work\repometric\Lints_Test",
                //    @"--file=D:\work\repometric\Lints_Test\Test_Js\easing.js",
                //    @"--linter=jshint"
                //};
                //Run(test3, log);

                //string[] test4 = {
                //    @"--mode=Analyze",
                //    @"--project=D:\work\repometric\Lints_Test",
                //    @"--linter=jshint"
                //};

                //Run(test4, log);
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
            //Strategies.Add(RunMode.Help, optionsStrategy);
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
