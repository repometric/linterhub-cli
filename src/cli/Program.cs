namespace Linterhub.Cli
{
    using System;
    using System.Collections.Generic;
    using Core.Schema;
    using Core.Runtime;
    using Core.Factory;
    using Core.Extensions;
    using Core.Exceptions;
    using Runtime;
    using Strategy;

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
            { RunMode.AnalyzeStdin, new AnalyzeStdinStrategy() },
            { RunMode.Version, new VersionStrategy() },
            { RunMode.Activate, new ActivateStrategy() },
            { RunMode.Help, new OptionsStrategy() },
            { RunMode.EngineVersion, new EngineVersionStrategy() },
            { RunMode.EngineInstall, new EngineInstallStrategy() },
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
                    if (string.IsNullOrEmpty(context.ProjectConfig))
                        throw new LinterhubException(
                            title: "Linterhub config",
                            description: "Cant save config cause no project path specified",
                            statusCode: LinterhubException.ErrorCode.linterhubConfig,
                            innerException: null);

                    var content = locator.Get<LinterhubConfigSchema>().SerializeAsJson();
                    System.IO.File.WriteAllText(context.ProjectConfig, content);
                }

                var output = result is string || result == null
                           ? result
                           : result.SerializeAsJson(context.Keys, context.Filters);

                if (output != null)
                {
                    Console.WriteLine(output);
                }
            }
            catch (LinterhubException exception)
            {
                Console.WriteLine(exception.Message);
                Environment.Exit((int)exception.exitCode);
            }
        }

        private static ServiceLocator RegisterServices(RunContext context)
        {
            var locator = new ServiceLocator();
            
            var platformConfig = context.PlatformConfig.DeserializeAsJsonFromFile<PlatformConfig>();
            var projectConfig = context.ProjectConfig.DeserializeAsJsonFromFile<LinterhubConfigSchema>();
            var terminal = new TerminalWrapper(platformConfig.Terminal.Path, platformConfig.Terminal.Command);
            var engineFactory = new EngineFileSystemFactory(context.Linterhub);
            var engineContextFactory = new EngineContextFactory(engineFactory);
            var commandFactory = new CommandFactory();
            var installer = new Installer(terminal, platformConfig.Command.Installed);
            var engineRunner = new EngineWrapper(terminal, commandFactory);

            locator.Register<LinterhubConfigSchema>(projectConfig);
            locator.Register<RunContext>(context);
            locator.Register<PlatformConfig>(platformConfig);
            locator.Register<IEngineFactory>(engineFactory);
            locator.Register<EngineContextFactory>(engineContextFactory);
            locator.Register<TerminalWrapper>(terminal);
            locator.Register<EngineWrapper>(engineRunner);
            locator.Register<Installer>(installer);

            var ensure = new Ensure(locator);
            locator.Register<Ensure>(ensure);

            return locator;
        }
    }
}