namespace Linterhub.Cli.Strategy
{
    using System;
    using System.IO;
    using System.Text;
    using System.Collections.Generic;
    using Runtime;
    using Engine;
    using Engine.Exceptions;
    using System.Linq;
    using Engine.Extensions;
    using Newtonsoft.Json;

    public class AnalyzeStrategy : IStrategy
    {
        public object Run(RunContext context, LinterEngine engine, LogManager log)
        {
            var result = string.Empty;
            var linterModels = new List<ILinterModel>();

            try
            {
                Stream input;
                if (!context.InputAwailable)
                {
                    var configs = GetCommands(context, engine, log);
                    foreach (var linterConfig in configs.Linters)
                    {
                        result = new LinterhubWrapper(context, engine).Analyze(linterConfig.Name, linterConfig.Command, context.Project);
                        input = new MemoryStream(Encoding.UTF8.GetBytes(result)) { Position = 0 };
                        //TODO: не должно ложиться от ошибки одного линтера
                        linterModels.Add(engine.GetModel(linterConfig.Name, input, null));
                    }
                }
                else
                {
                    input = context.Input;
                    linterModels.Add(engine.GetModel(context.Linter, input, null));
                }
            }
            catch (Exception exception)
            {
                throw new LinterEngineException(result + " " + exception.Message, exception);
            }

            return linterModels;
        }

        private ExtConfig GetCommands(RunContext context, LinterEngine engine, LogManager log)
        {
            var projectConfigFile = Path.Combine(context.Project, ".linterhub.json");
            var linter = context.Linter ?? string.Empty;
            var extConfig = new ExtConfig();

            log.Trace("Expected project config: " + projectConfigFile);

            if (File.Exists(projectConfigFile))
            {
                log.Trace("Using project config");
                try
                {
                    using (var fs = File.Open(projectConfigFile, FileMode.Open))
                    {
                        extConfig = fs.DeserializeAsJson<ExtConfig>();
                       
                        if (linter == string.Empty)
                        {
                            foreach (var thisLinter in extConfig.Linters.Where(x => x.Config != null))
                            {
                                thisLinter.Command = GetCommand(thisLinter, engine);
                            }
                        }
                        else
                        {
                            var linterConfig = extConfig.Linters.FirstOrDefault(x => x.Name == linter);
                            if (linterConfig == null)
                            {
                                throw new LinterNotFoundException("Can't find linter with name: " + linter);
                            }
                            linterConfig.Command = GetCommand(linterConfig, engine);
                        }
                        
                    }
                }
                catch (Exception exception)
                {
                    throw new LinterEngineException("Error parsing project configuration file", exception);
                }
            }
            else
            {
                log.Trace("Project config was not found");
                extConfig.Linters[0].Command = engine.Factory.GetArguments(context.Linter);
                extConfig.Linters[0].Name = context.Linter;
            }

            return extConfig;
        }

        public string GetCommand(ExtConfig.ExtLint extLint, LinterEngine engine)
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(extLint.Config)));
            var args = engine.GetArguments(extLint.Name, stream);
            return extLint.Command ?? engine.Factory.CreateCommand(args);
        }
    }
}