namespace Linterhub.Cli.Strategy
{
    using System;
    using System.IO;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Runtime;
    using Engine;
    using Engine.Extensions;
    using Engine.Exceptions;
    using Engine.Linters;
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
                    foreach (var linterConfig in configs.Linters.Where(x => x.Active == true || x.Active == null))
                    {
                        result = new LinterhubWrapper(context, engine).Analyze(linterConfig.Name, linterConfig.Command, context.Project);
                        input = MemoryStreamUtf8(result);
                        input.Position = 0;
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
            var linterName = context.Linter ?? string.Empty;
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
            }

            extConfig = linterName == string.Empty ?
                    GetLinters(extConfig, engine) :
                    GetLinter(linterName, extConfig, engine);

            return extConfig;
        }

        public ExtConfig GetLinters(ExtConfig config, LinterEngine engine)
        {
            if (config.Linters.Count == 0)
            {
                var defaultLinters = Registry.Get().Where(x => x.ArgsDefault);

                foreach (var linter in defaultLinters)
                {
                    config.Linters.Add(new ExtConfig.ExtLint
                    {
                        Command = engine.Factory.GetArguments(linter.Name),
                        Name = linter.Name
                    });
                }
            }
            else
            {
                foreach (var thisLinter in config.Linters.Where(x => x.Config != null))
                {
                    thisLinter.Command = GetCommand(thisLinter, engine);
                }
            }
            
            return config;
        }
        public ExtConfig GetLinter(string name, ExtConfig config, LinterEngine engine)
        {
            var thisLinter = config.Linters.FirstOrDefault(x => x.Name == name);
            if (thisLinter == null)
            {
                var findLinter = Registry.Get(name);
                if (findLinter == null)
                {
                    throw new LinterNotFoundException("Can't find linter with name: " + name);
                }
                config.Linters.Add(new ExtConfig.ExtLint
                {
                    Command = engine.Factory.GetArguments(findLinter.Name),
                    Name = findLinter.Name
                });
            }
            else
            {
                thisLinter.Command = GetCommand(thisLinter, engine);
            }
            return config;
        }

        public string GetCommand(ExtConfig.ExtLint extLint, LinterEngine engine)
        {
            var stream = MemoryStreamUtf8(JsonConvert.SerializeObject(extLint.Config));
            var args = engine.GetArguments(extLint.Name, stream);
            return extLint.Command ?? engine.Factory.CreateCommand(args);
        }

        public Stream MemoryStreamUtf8(string str)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(str));
        }
    }
}