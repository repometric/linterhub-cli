namespace Linterhub.Cli.Strategy
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Linq;
    using Runtime;
    using Engine;
    using Engine.Exceptions;
    using Engine.Extensions;

    //// TODO: Refactor this code!
    public class AnalyzeStrategy : IStrategy
    {
        public object Run(RunContext context, LinterFactory factory, LogManager log)
        {
            var result = string.Empty;
            var linterModels = new List<ILinterModel>();
            try
            {
                Stream input;
                if (!context.InputAwailable)
                {
                    var configs = GetConfigs(context, factory, log);
                    foreach (var linterConfig in configs.Linters.Where(x => x.Command != null))
                    {
                        result = new LinterhubWrapper(context).Analyze(linterConfig.Name, linterConfig.Command, context.Project);
                        using (input = result.GetMemoryStream())
                        {
                            linterModels.Add(factory.CreateModel(linterConfig.Name, input, null));
                        }
                    }
                }
                else
                {
                    input = context.Input;
                    linterModels.Add(factory.CreateModel(context.Linter, input, null));
                }
            }
            catch (Exception exception)
            {
                // TODO: If single linter failed - don't stop others.
                throw new LinterEngineException(result + " " + exception.Message, exception);
            }
            
            return linterModels;
        }

        private ProjectConfig GetConfigs(RunContext context, LinterFactory factory, LogManager log)
        {
            var projectConfigFile = context.GetProjectConfigPath();
            var existFileConfig = File.Exists(projectConfigFile);
            log.Trace("Expected project config: " + projectConfigFile);
            var config = context.GetProjectConfig();
            var mode = !string.IsNullOrEmpty(context.File) ? ArgMode.File : ArgMode.Folder;
            var isLinterSpecified = !string.IsNullOrEmpty(context.Linter);
            return isLinterSpecified
                ? GetLinter(context, factory, context.Linter, config, mode):
                    GetLinters(context, factory, config, existFileConfig, mode);
        }

        public ProjectConfig GetLinters(RunContext context, LinterFactory factory, ProjectConfig config, bool fileConfig, ArgMode mode)
        {
            foreach (var thisLinter in config.Linters.Where(x=> x.Active != false))
            {
                var path = context.GetAnalyzePath();
                thisLinter.Command = GetCommand(factory, thisLinter, context.Project, path, mode, fileConfig);
            }

            return config;
        }

        public ProjectConfig GetLinter(RunContext context, LinterFactory factory, string name, ProjectConfig config, ArgMode mode)
        {
            var thisLinter = config.Linters.FirstOrDefault(x => x.Name == name);
            if (thisLinter == null)
            {
                var findLinter = factory.GetRecord(name);
                config.Linters.Add(new ProjectConfig.Linter
                {
                    Command = factory.BuildCommand(findLinter.Name, context.Project, context.GetAnalyzePath(), mode),
                    Name = findLinter.Name,
                });
            }
            else
            {
               thisLinter.Command = thisLinter.Command ?? 
                    (thisLinter.Active != false ?
                    factory.BuildCommand(thisLinter.Name, context.Project, context.GetAnalyzePath(), mode) : null);
            }
            return config;
        }
 
        public string GetCommand(LinterFactory factory, ProjectConfig.Linter linter, string path, ArgMode mode)
        {
            var stream = linter.Config.SerializeAsJson().GetMemoryStream();
            var args = factory.CreateArguments(linter.Name, stream);
            return linter.Command ?? factory.BuildCommand(args, path, path, mode);
        }

        public string GetCommand(LinterFactory factory, ProjectConfig.Linter linter, string workDir, string path, ArgMode mode, bool fileConfig)
        {

            return linter.Command ??
                   (fileConfig
                       ? GetCommand(factory, linter, path, mode)
                       : factory.BuildCommand(linter.Name, workDir, path, mode));
        }
    }
}