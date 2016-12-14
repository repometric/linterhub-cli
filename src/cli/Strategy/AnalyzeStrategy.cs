namespace Linterhub.Cli.Strategy
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Linq;
    using Runtime;
    using Engine;
    using Engine.Exceptions;
    using Engine.Linters;
    using Newtonsoft.Json;

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
                    var configs = GetCommands(context, factory, log);
                  
                    foreach (var linterConfig in configs.Linters.Where(x => x.Command != null))
                    {
                        result = new LinterhubWrapper(context).Analyze(linterConfig.Name, linterConfig.Command, context.Project);
                        using (input = result.GetMemoryStream())
                        {
                            //input.Position = 0;
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
                throw new LinterEngineException(result + " " + exception.Message, exception);
            }
            
            return linterModels;
        }

        private ProjectConfig GetCommands(RunContext context, LinterFactory factory, LogManager log)
        {
            var projectConfigFile = context.GetProjectConfigPath();
            log.Trace("Expected project config: " + projectConfigFile);
            var config = context.GetProjectConfig();

            var existFileConfig = File.Exists(projectConfigFile);
            var mode = !string.IsNullOrEmpty(context.File) ? ArgMode.File : ArgMode.Folder;
            config = string.IsNullOrEmpty(context.Linter) ?
                    GetLinters(context, factory, config, existFileConfig, mode) :
                    GetLinter(context, factory, context.Linter, config, mode);

            return config;
        }

        public ProjectConfig GetLinters(RunContext context, LinterFactory factory, ProjectConfig config, bool fileConfig, ArgMode mode)
        {
            foreach (var thisLinter in config.Linters.Where(x=> x.Active != false))
            {
                var path = context.GetAnalyzePath();
                thisLinter.Command = thisLinter.Command ??
                                     (fileConfig
                                         ? GetCommand(factory, thisLinter, path, mode)
                                         : factory.BuildCommand(thisLinter.Name, context.Project, path, mode));
            }
            return config;
        }

        public ProjectConfig GetLinter(RunContext context, LinterFactory factory, string name, ProjectConfig config, ArgMode mode)
        {
            var thisLinter = config.Linters.FirstOrDefault(x => x.Name == name);
            if (thisLinter == null)
            {
                var findLinter = factory.GetRecord(name);
                if (findLinter == null)
                {
                    throw new LinterNotFoundException("Can't find linter with name: " + name);
                }

                config.Linters.Add(new ProjectConfig.Linter
                {
                    Command = factory.BuildCommand(findLinter.Name, context.Project, context.GetAnalyzePath(), mode),
                    Name = findLinter.Name,
                });
            }
            else
            {
               thisLinter.Command = thisLinter.Command ?? 
                    (thisLinter.Active == true || thisLinter.Active == null ?
                    factory.BuildCommand(thisLinter.Name, context.Project, context.GetAnalyzePath(), mode) : null);
            }
            return config;
        }
 
        public string GetCommand(LinterFactory factory, ProjectConfig.Linter linter, string path, ArgMode mode)
        {
            var stream = JsonConvert.SerializeObject(linter.Config).GetMemoryStream();
            var args = factory.CreateArguments(linter.Name, stream);
            return linter.Command ?? factory.BuildCommand(args, path, path, mode);
        }
    }
}