namespace Linterhub.Cli.Strategy
{
    using System;
    using System.Collections.Generic;
    using Mono.Options;
    using Runtime;
    using ServiceLocator = Runtime.ServiceLocator;
    using Core.Exceptions;

    public class OptionsStrategy : IStrategy
    {
        public OptionSet Options { get; private set; }

        public RunContext Parse(string[] args)
        {
            var runContext = new RunContext();
            Options = new OptionSet
            {
                { "c=|config=", "Path to configuration.", v => runContext.ProjectConfig = v },
                { "s=|system=", "Path to platform configuration.", v => runContext.PlatformConfig = v },
                { "e=|engine=", "The engine name.", v => runContext.Engines = string.IsNullOrEmpty(v) ? new string[0] : v.Split(',') },
                { "p=|project=", "Path to project.", v => runContext.Project = v },
                { "d=|folder=", "Path to directory", v => runContext.Directory = v },
                { "f=|file=", "Path to file.", v => runContext.File = v },
                { "m=|mode=", "Run mode.", v => runContext.Mode = (RunMode)Enum.Parse(typeof(RunMode), v, true) },
                { "locally=", "Run/Install engine locally or globally (locally by default)", v => runContext.Locally = bool.Parse(v) },
                { "l=|line=", "Line in a file.", v => runContext.Line = int.Parse(v) },
                { "r=|ruleid=", "Rule id.", v => runContext.RuleId = v },
                { "stdin", "Use stdin for analyze.", v => runContext.InputAwailable = true },
                { "k=|keys=", "Keys to include.", v => runContext.Keys = string.IsNullOrEmpty(v) ? new string[0] : v.Replace("\"", "").Replace("'", "").Split(',') },
                { "filters=", "Filters to apply", v => runContext.Filters = string.IsNullOrEmpty(v) ? new string[0] : v.Replace("\"", "").Replace("'", "").Split(',') },
                { "linterhub=", "Path to linterhub.", v => runContext.Linterhub = v },
                { "h|help",  "Show help.", v => runContext.Mode = RunMode.Help },
            };

            List<string> extra = null;
            try
            {
                extra = Options.Parse(args);
            }
            catch (Exception exception)
            {
                throw new EngineException("Error parsing arguments", exception.Message);
            }

            runContext.Input = Console.OpenStandardInput();
            return runContext;
        }

        public object Run(ServiceLocator locator)
        {
            Console.WriteLine("Options:");
            Options.WriteOptionDescriptions(Console.Out);
            return null;
        }
    }
}