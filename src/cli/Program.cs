namespace Linterhub.Cli
{
    using System;
    using System.Collections.Generic;
    using Core.Schema;
    using Core.Runtime;
    using Core.Factory;
    using Core.Utils;
    using Core.Exceptions;
    using Strategy;
    using Runtime;
    using System.IO;
    using Core.Managers;

    internal class Program
    {
        internal static int Main(string[] args)
        {
            using (var log = new LogManager())
            {
                return Run(args, log);
            }
        }

        internal static Dictionary<RunMode, IStrategy> Strategies = new Dictionary<RunMode, IStrategy>
        {
            { RunMode.Catalog, new CatalogStrategy() },
            { RunMode.Analyze, new AnalyzeStrategy() },
            { RunMode.AnalyzeStdin, new AnalyzeStdinStrategy() },
            { RunMode.Version, new VersionStrategy() },
            { RunMode.Activate, new ActivateStrategy() },
            { RunMode.Deactivate, new ActivateStrategy() },
            { RunMode.Help, new OptionsStrategy() },
            { RunMode.Ignore, new IgnoreStrategy() },
            { RunMode.Fetch, new FetchEnginesStrategy() }
        };
        
        internal static int Run(string[] args, LogManager log)
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
                    {
                        throw new LinterhubException(
                            title: "Linterhub config",
                            description: "Cant save config cause no project path specified",
                            statusCode: LinterhubException.ErrorCode.linterhubConfig,
                            innerException: null);
                    }

                    var content = locator.Get<LinterhubConfigSchema>().SerializeAsJson();
                    File.WriteAllText(context.ProjectConfig, content);
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
                return (int)exception.exitCode;
            }
            return 0;
        }

        private static ServiceLocator RegisterServices(RunContext context)
        {
            var locator = new ServiceLocator();
            
            var platformConfig = context.PlatformConfig.DeserializeAsJsonFromFile<PlatformConfig>();
            var projectConfig = context.ProjectConfig.DeserializeAsJsonFromFile<LinterhubConfigSchema>();
            var terminal = new TerminalWrapper(platformConfig.Terminal.Path, platformConfig.Terminal.Command);
            var engineFactory = new EngineFileSystemFactory(context.Linterhub);
            var commandFactory = new CommandFactory();
            var managerWrapper = new ManagerWrapper(terminal, engineFactory);
            var installer = new Installer(managerWrapper);
            var engineRunner = new EngineWrapper(terminal, commandFactory, managerWrapper);

            locator.Register<LinterhubConfigSchema>(projectConfig);
            locator.Register<RunContext>(context);
            locator.Register<PlatformConfig>(platformConfig);
            locator.Register<IEngineFactory>(engineFactory);
            locator.Register<TerminalWrapper>(terminal);
            locator.Register<EngineWrapper>(engineRunner);
            locator.Register<Installer>(installer);
            locator.Register<ManagerWrapper>(managerWrapper);

            var ensure = new Ensure(locator);
            locator.Register<Ensure>(ensure);

            return locator;
        }
    }
}