namespace Linterhub.Cli.Strategy
{
    using System;
    using System.IO;
    using System.Text;
    using Linterhub.Cli.Runtime;
    using Linterhub.Engine;
    using Linterhub.Engine.Exceptions;

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
            var projectConfigFile = Path.Combine(context.Project, context.Configuration.ProjectConfig, context.Linter ?? "", "config.json");
            log.Trace("Expected project config: " + projectConfigFile);
            string command;

            if (File.Exists(projectConfigFile))
            {
                log.Trace("Using project config");
                string linterConfiguration;
                try
                {
                    linterConfiguration = File.ReadAllText(projectConfigFile);
                }
                catch (Exception exception)
                {
                    throw new LinterEngineException("Error reading project configuration file", exception);
                }

                var stream = new MemoryStream(Encoding.UTF8.GetBytes(linterConfiguration));
                var args = engine.GetArguments(context.Linter, stream);
                command = engine.Factory.CreateCommand(args);
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