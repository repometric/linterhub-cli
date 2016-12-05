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
        public Guid TempDirName;
        public string TempDirPath;
        public RunContext Context;
        public LinterEngine Engine;
        public LogManager Log;
        public object Run(RunContext context, LinterEngine engine, LogManager log)
        {
            Context = context;
            Engine = engine;
            Log = log;
            TempDirName = Guid.NewGuid();

            var result = TempDirPath = string.Empty;
            var linterModels = new List<ILinterModel>();
            try
            {
                Stream input;
                if (!Context.InputAwailable)
                {
                    var configs = GetCommands();
                  
                    foreach (var linterConfig in configs.Linters.Where(x => x.Command != null))
                    {
                        result = new LinterhubWrapper(Context, Engine).Analyze(linterConfig.Name, linterConfig.Command, Context.Project);
                        input = result.GetMemoryStream();
                        input.Position = 0;
                        linterModels.Add(Engine.GetModel(linterConfig.Name, input, null));
                    }
                    if (!string.IsNullOrEmpty(TempDirPath))
                    {
                        Directory.Delete(TempDirPath, true);
                    }
                }
                else
                {
                    input = Context.Input;
                    linterModels.Add(Engine.GetModel(Context.Linter, input, null));
                }
            }
            catch (Exception exception)
            {
                if (!string.IsNullOrEmpty(TempDirPath))
                {
                    Directory.Delete(TempDirPath, true);
                }
                throw new LinterEngineException(result + " " + exception.Message, exception);
            }
            
            return linterModels;
        }

        private ExtConfig GetCommands()
        {
            var projectConfigFile = Path.Combine(Context.Project, ".linterhub.json");
            var linterName = Context.Linter ?? string.Empty;
            var extConfig = new ExtConfig();
            
            Log.Trace("Expected project config: " + projectConfigFile);

            var existFileConfig = File.Exists(projectConfigFile);
            if (existFileConfig)
            {
                Log.Trace("Using project config");
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
                Log.Trace("Project file config was not found. Config file was generated automatically");
                var defaultLinters = Registry.Get().Where(x => x.ArgsDefault);
                
                foreach (var linter in defaultLinters)
                {                  
                    extConfig.Linters.Add(new ExtConfig.ExtLint
                    {
                        Name = linter.Name
                    });
                }
            }

            extConfig = linterName == string.Empty ?
                    GetLinters(extConfig, existFileConfig) :
                    GetLinter(linterName, extConfig);

            return extConfig;
        }

        public string GetPath(string name)
        {
            if (string.IsNullOrEmpty(Context.File)) return Context.Dir.Replace('\\', '/') ?? "./";

            var defaultLinters = Registry.Get().SingleOrDefault(x => x.OneFile && x.Name == name);
            if (defaultLinters != null){ return Context.File; }

            var filePathSplit = Context.File.Split('.');
            var filePermission = filePathSplit[filePathSplit.Length - 1];
            TempDirPath = CreateTempCatalog(Context.Project, TempDirName);

            var localFile = Path.Combine(Context.Project, Context.File);
            var tempFile = Path.Combine(TempDirPath, "temp." + filePermission);

            if (!File.Exists(tempFile)){ File.Copy(localFile, tempFile); }
            return "./" + TempDirName + "/";
        }
        public ExtConfig GetLinters(ExtConfig config, bool fileConfig)
        {
            foreach (var thisLinter in config.Linters.Where(x=> x.Active == true || x.Active == null))
            {
                var path = GetPath(thisLinter.Name);
                thisLinter.Command = thisLinter.Command ??
                                     (fileConfig
                                         ? GetCommand(thisLinter, path)
                                         : Engine.Factory.GetArguments(thisLinter.Name, path));
            }
            return config;
        }
        public ExtConfig GetLinter(string name, ExtConfig config)
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
                    Command = Engine.Factory.GetArguments(findLinter.Name, GetPath(findLinter.Name)),
                    Name = findLinter.Name,
                });
            }
            else
            {
               thisLinter.Command = thisLinter.Command ?? 
                    (thisLinter.Active == true || thisLinter.Active == null ?
                    Engine.Factory.GetArguments(thisLinter.Name, GetPath(thisLinter.Name)) : null);
            }
            return config;
        }

        public string GetCommand(ExtConfig.ExtLint extLint, string path)
        {
            var stream = JsonConvert.SerializeObject(extLint.Config).GetMemoryStream();
            var args = Engine.GetArguments(extLint.Name, stream);
            return extLint.Command ?? Engine.Factory.CreateCommand(args, path);
        }

        public string CreateTempCatalog(string path, Guid guid)
        {
            var tempDirPath = path + "\\" + guid + "\\";
            Directory.CreateDirectory(tempDirPath);
            return tempDirPath;
        }
    }
}