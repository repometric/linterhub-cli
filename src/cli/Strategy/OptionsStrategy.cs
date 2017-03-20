namespace Linterhub.Cli.Strategy
{
    using System;
    using System.Collections.Generic;
    using Mono.Options;
    using Runtime;
    using Linterhub.Engine.Exceptions;
    using System.Text.RegularExpressions;

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
                { "l=|linter=", "The linter name.", v => runContext.Linters = string.IsNullOrEmpty(v) ? new string[0] : v.Split(',') },
                { "p=|project=", "Path to project.", v => runContext.Project = v },
                { "d=|folder=", "Path to directory", v => runContext.Directory = v },
                { "f=|file=", "Path to file.", v => runContext.File = v },
                { "m=|mode=", "Run mode.", v => runContext.Mode = (RunMode)Enum.Parse(typeof(RunMode), v, true) },
                { "a=|active=", "Activate or not.", v => runContext.Activate = bool.Parse(v) },
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
                throw new LinterEngineException("Error parsing arguments", exception);
            }

            /*if (extra.Any())
            {
                throw new LinterEngineException("Extra arguments: " + string.Join(",", extra));
            }*/
            /*
            Dictionary<string, string> extraArgs = new Dictionary<string, string>();
            foreach (string arg in extra)
            {
                string pattern = @"([a-zA-Z0-9.])+";
                Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
                MatchCollection matches = rgx.Matches(arg);
                extraArgs.Add(matches[0].Value, matches[1].Value);
            }

            runContext.ExtraArgs = extraArgs;
            */
            runContext.Input = Console.OpenStandardInput();
            runContext.InputAwailable = false; //Console.IsInputRedirected;
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