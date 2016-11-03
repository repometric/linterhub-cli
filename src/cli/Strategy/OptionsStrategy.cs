namespace Linterhub.Cli.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Mono.Options;
    using Linterhub.Cli.Runtime;
    using Linterhub.Engine;
    using Linterhub.Engine.Exceptions;

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
                { "m=|mode=", "Run mode.", v => runContext.Mode = (RunMode)Enum.Parse(typeof(RunMode), v, true) },
                { "h|help",  "Show help.", v => runContext.Mode = RunMode.Help },
            };

            List<string> extra;
            try
            {
                extra = Options.Parse(args);
            }
            catch (Exception exception)
            {
                throw new LinterException("Error parsing arguments: ", exception.Message);
            }

            if (extra.Any())
            {
                throw new LinterException("Extra arguments: ", string.Join(",", extra));
            }

            runContext.Input = Console.OpenStandardInput();
            runContext.InputAwailable = System.Console.IsInputRedirected;
            return runContext;
        }

        public object Run(RunContext context, LinterEngine engine)
        {
            Console.WriteLine("Options:");
            Options.WriteOptionDescriptions(Console.Out);
            return null;
        }
    }
}