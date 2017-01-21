namespace Linterhub.Cli.Strategy
{
    using Runtime;
    using Engine;
    using Linterhub.Engine.Exceptions;
    using Newtonsoft.Json;
    using System.Dynamic;

    public class LinterVersionStrategy : IStrategy
    {

        private const int LINTER_DEXIST = 152;
        public object Run(RunContext context, LinterFactory factory, LogManager log)
        {
            if (string.IsNullOrEmpty(context.Linter))
            {
                throw new LinterEngineException("Linter is not specified: " + context.Linter);
            }
            dynamic result = new ExpandoObject();
            var versionCmd = factory.BuildVersionCommand(context.Linter);

            var runResults = new LinterhubWrapper(context).LinterVersion(context.Linter, versionCmd);
            var version = runResults.Output?.ToString().Trim();
            var exitCode = runResults.ExitCode;

            result.LinterName = context.Linter;
            result.Installed = (exitCode == LINTER_DEXIST) ? false : true;
            result.Version = !result.Installed ? "Unknown" : version;

            return JsonConvert.SerializeObject(result);
        }

        
    }
}