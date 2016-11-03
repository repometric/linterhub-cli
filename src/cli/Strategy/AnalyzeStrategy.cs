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
        public object Run(RunContext context, LinterEngine engine)
        {
            try
            {
                Stream input;
                if (!context.InputAwailable)
                {
                    var command = GetCommand(context, engine);
                    var result = new LinterhubWrapper(context, engine).Analyze(context.Linter, command, context.Project);
                    input = new MemoryStream(Encoding.UTF8.GetBytes(result));
                    input.Position = 0;
                }
                else
                {
                    input = context.Input;
                }

                return engine.GetModel(context.Linter, input, null);
            }
            catch (Exception exception)
            {
                throw new LinterParseException(exception);
            }
        }

        private string GetCommand(RunContext context, LinterEngine engine)
        {
            var linterContext = new LinterContext(context.Configuration, context);
            var linterConfigFile = linterContext.GetLinterConfigFile();
            if (!File.Exists(linterConfigFile))
            {
                throw new LinterException("Linter configuration was not found: ", linterConfigFile);
            }

            string linterConfiguration;
            try
            {
                linterConfiguration = File.ReadAllText(linterConfigFile);
            }
            catch (Exception e)
            {
                throw new LinterException("Error reading linter configuration file: " + e.Message);
            }

            var args = engine.GetArguments(context.Linter, new MemoryStream(Encoding.UTF8.GetBytes(linterConfiguration)));
            return engine.GetLinterCommand(args);
        }
    }
}