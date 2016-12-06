namespace Linterhub.Cli.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Mono.Options;
    using Runtime;
    using Engine;
    using Engine.Exceptions;

    public class OptionsStrategy : IStrategy
    {
        public OptionSet Options { get; private set; }

        public RunContext Parse(string[] args)
        {
            var runContext = new RunContext();
            Options = new OptionSet
            {
                { "c=|config=", "Path to configuration.", v => runContext.Config = v },
                { "l=|linter=", "The linter name.", v => runContext.Linter = v },
                { "p=|project=", "Path to project.", v => runContext.Project = v },
                { "f=|file=", "File of project.", v => runContext.File = v },
                { "d=|dir=", "Dir of project.", v => runContext.Dir = v },
                { "m=|mode=", "Run mode.", v => runContext.Mode = (RunMode)Enum.Parse(typeof(RunMode), v, true) },
                { "a=|active=", "Activate or not", v => runContext.Activate = bool.Parse(v) },
                { "h|help",  "Show help.", v => runContext.Mode = RunMode.Help },
            };

            List<string> extra;
            try
            {
                extra = Options.Parse(args);
            }
            catch (Exception exception)
            {
                throw new LinterEngineException("Error parsing arguments", exception);
            }

            if (extra.Any())
            {
                throw new LinterEngineException("Extra arguments: " + string.Join(",", extra));
            }

            runContext.Input = Console.OpenStandardInput();
            runContext.InputAwailable = false; //Console.IsInputRedirected;
            return runContext;
        }

        public object Run(RunContext context, LinterEngine engine, LogManager log)
        {
            Console.WriteLine("Options:");
            Options.WriteOptionDescriptions(Console.Out);
            return null;
        }
    }
}