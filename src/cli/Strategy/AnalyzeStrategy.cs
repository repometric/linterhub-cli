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
        public static string TempDir;
        public object Run(RunContext context, LinterEngine engine, LogManager log)
        {
            var result = TempDir = string.Empty;
            var linterModels = new List<ILinterModel>();
            
            try
            {
                Stream input;
                if (!context.InputAwailable)
                {
                    var configs = GetCommands(context, engine, log);
                  
                    foreach (var linterConfig in configs.Linters.Where(x => x.Command != null))
                    {
                        result = new LinterhubWrapper(context, engine).Analyze(linterConfig.Name, linterConfig.Command, context.Project);
                        input = MemoryStreamUtf8(result);
                        input.Position = 0;
                        linterModels.Add(engine.GetModel(linterConfig.Name, input, null));
                    }
                    if(!string.IsNullOrEmpty(TempDir))
                    {
                        Directory.Delete(TempDir, true);
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
                if (!string.IsNullOrEmpty(TempDir))
                {
                    Directory.Delete(TempDir, true);
                }
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

            var existFileConfig = File.Exists(projectConfigFile);
            if (existFileConfig)
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
                log.Trace("Project config was not found. Config generate auotomat");
                var defaultLinters = Registry.Get().Where(x => x.ArgsDefault);
                
                foreach (var linter in defaultLinters)
                {                  
                    extConfig.Linters.Add(new ExtConfig.ExtLint
                    {
                        Name = linter.Name
                    });
                }
            }

            extConfig = GetTestPathDocker(context, extConfig, ref TempDir);

            extConfig = linterName == string.Empty ?
                    GetLinters(extConfig, engine, !existFileConfig) :
                    GetLinter(linterName, extConfig, engine, context, ref TempDir);

            return extConfig;
        }

        public ExtConfig GetTestPathDocker(RunContext context, ExtConfig config, ref string tempDir)
        {
            if (string.IsNullOrEmpty(context.File)) return config;

            var defaultLinters = Registry.Get().Where(x => x.OneFile).ToList();
            var tempDirName = Guid.NewGuid();
            var findDefaultLinter = false;

            foreach (var linter in config.Linters)
            {
                var defaultLinter = defaultLinters.Find(x => x.Name == linter.Name);

                if (defaultLinter == null)
                {
                    findDefaultLinter = true;
                    linter.Config.TestPathDocker = "./" + tempDirName + "/";
                }
                else
                {
                    linter.Config.TestPathDocker = context.File.Replace(context.Project, ".").Replace("\\", "/");
                }
            }

            if (!findDefaultLinter) return config;

            var catalogSrategy = new CatalogStrategy();
            var filePathSplit = context.File.Split('.');
            var filePermission = filePathSplit[filePathSplit.Length - 1];

            tempDir = catalogSrategy.CreeateTempCatalog(context.Project, tempDirName);
            File.Copy(context.File, TempDir + "\\temp." + filePermission);
            return config;
        }
        public ExtConfig GetLinters(ExtConfig config, LinterEngine engine, bool changePathDocker = false)
        {
            foreach (var thisLinter in config.Linters.Where(x=> x.Active == true || x.Active == null))
            {
                thisLinter.Command = engine.Factory.GetArguments(thisLinter.Name, thisLinter.Config.TestPathDocker);
            }
            return config;
        }
        public ExtConfig GetLinter(string name, ExtConfig config, LinterEngine engine, RunContext context, ref string tempDir)
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
                    Name = findLinter.Name,
                });
                config = GetTestPathDocker(context, config, ref tempDir);
            }
            else
            {
                thisLinter.Command = thisLinter.Active == true || thisLinter.Active == null ?
                    engine.Factory.GetArguments(thisLinter.Name, thisLinter.Config.TestPathDocker) : null;

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