namespace Linterhub.Cli
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Linterhub.Engine.Schema;
    using Linterhub.Engine.Runtime;
    using Linterhub.Engine.Factory;
    using Linterhub.Engine.Extensions;
    using Linterhub.Cli.Runtime;
    using Linterhub.Cli.Strategy;

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
            { RunMode.Analyze, new AnalyzeStrategy() },
            { RunMode.Version, new VersionStrategy() },
            { RunMode.Activate, new ActivateStrategy() },
            { RunMode.Help, new OptionsStrategy() },
            { RunMode.LinterVersion, new LinterVersionStrategy() },
            { RunMode.LinterInstall, new LinterInstallStrategy() },
            { RunMode.Ignore, new IgnoreStrategy() }
        };
        
        internal static void Run(string[] args, LogManager log)
        {
            try
            {
                var optionsStrategy = (OptionsStrategy)Strategies[RunMode.Help];
                var context = optionsStrategy.Parse(args);
                var locator = new ServiceLocator();
                if (context.Mode != RunMode.Help)
                {
                    locator.Register<RunContext>(context);
                    locator.Register<Ensure>(new Ensure(locator));
                    var validateStrategy = new ValidateStrategy();
                    validateStrategy.Run(locator);
                    locator = RegisterServices(context);
                }

                var result = Strategies[context.Mode].Run(locator);
                if (context.SaveConfig)
                {
                    var content = locator.Get<LinterhubConfigSchema>().SerializeAsJson();
                    System.IO.File.WriteAllText(string.IsNullOrEmpty(context.ProjectConfig) ? "linterhub.json" : context.ProjectConfig, content);
                }

                var output = result is string || result == null
                           ? result
                           : result.SerializeAsJson(context.Keys, context.Filters);

                if (output != null)
                {
                    Console.WriteLine(output);
                }
            }
            catch (Exception exception)
            {
                log.Error(exception);
            }
        }

        private static ServiceLocator RegisterServices(RunContext context)
        {
            var locator = new ServiceLocator();
            
            var platformConfig = context.PlatformConfig.DeserializeAsJsonFromFile<PlatformConfig>();
            var projectConfig = context.ProjectConfig.DeserializeAsJsonFromFile<LinterhubConfigSchema>();
            var terminal = new TerminalWrapper(platformConfig.Terminal.Path, platformConfig.Terminal.Command);
            var linterFactory = new LinterFileSystemFactory(context.Linterhub);
            var linterContextFactory = new LinterContextFactory(linterFactory);
            var commandFactory = new CommandFactory();
            var installer = new Installer(terminal, platformConfig.Command.Installed);
            var linterRunner = new LinterWrapper(terminal, commandFactory);

            locator.Register<LinterhubConfigSchema>(projectConfig);
            locator.Register<RunContext>(context);
            locator.Register<PlatformConfig>(platformConfig);
            locator.Register<ILinterFactory>(linterFactory);
            locator.Register<LinterContextFactory>(linterContextFactory);
            locator.Register<TerminalWrapper>(terminal);
            locator.Register<LinterWrapper>(linterRunner);
            locator.Register<Installer>(installer);

            return locator;
        }
    }
}
