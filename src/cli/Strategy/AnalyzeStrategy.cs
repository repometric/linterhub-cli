namespace Linterhub.Cli.Strategy
{
    using System;
    using System.IO;
    using System.Text;
    using Linterhub.Cli.Runtime;
    using Linterhub.Engine;
    using Linterhub.Engine.Exceptions;
    using Newtonsoft.Json.Linq;
    using System.Linq;
    using Engine.Extensions;
    using Newtonsoft.Json;

    public class AnalyzeStrategy : IStrategy
    {
        public object Run(RunContext context, LinterEngine engine, LogManager log)
        {
            string result = string.Empty;
            Stream input;
            if (!context.InputAwailable)
            {
                string command = GetCommand(context, engine, log);
                result = new LinterhubWrapper(context, engine).Analyze(context.Linter, command, context.Project);
                input = new MemoryStream(Encoding.UTF8.GetBytes(result));
                input.Position = 0;
            }
            else
            {
                input = context.Input;
            }

            try
            {
                return engine.GetModel(context.Linter, input, null);
            }
            catch (Exception exception)
            {
                throw new LinterEngineException(result + " " + exception.Message, exception);
            }
        }

        private string GetCommand(RunContext context, LinterEngine engine, LogManager log)
        {
            var projectConfigFile = Path.Combine(context.Project, ".linterhub.json");
            var linter = context.Linter ?? "";
            
            log.Trace("Expected project config: " + projectConfigFile);
            string command;

            if (File.Exists(projectConfigFile))
            {
                log.Trace("Using project config");
                try
                {
                    FileStream fs = File.Open(projectConfigFile, FileMode.Open);
                    try
                    {
                        ExtConfig e = fs.DeserializeAsJson<ExtConfig>();
                        var a = e.Linters.Where(x => x.Name == linter).First().Config;
                        var stream = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(a)));
                        var args = engine.GetArguments(context.Linter, stream);
                        command = engine.Factory.CreateCommand(args);
                    }
                    catch (Exception exception)
                    {
                        throw new LinterEngineException("Error parsing project configuration file", exception);
                    }
                }
                catch (Exception exception)
                {
                    throw new LinterEngineException("Error reading project configuration file", exception);
                }
            }
            else
            {
                log.Trace("Project config was not found");
                command = engine.Factory.GetArguments(context.Linter);
            }

            return command;
        }
    }
}