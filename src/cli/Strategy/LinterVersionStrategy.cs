namespace Linterhub.Cli.Strategy
{
    using Runtime;
    using Engine;
    using Linterhub.Engine.Exceptions;
    using Newtonsoft.Json;
    using System.Dynamic;

    public class LinterVersionStrategy : IStrategy
    {
        public object Run(RunContext context, LinterFactory factory, LogManager log)
        {
            if (string.IsNullOrEmpty(context.Linter))
            {
                throw new LinterEngineException("Linter is not specified: " + context.Linter);
            }
            dynamic result = new ExpandoObject();
            var versionCmd = factory.BuildVersionCommand(context.Linter);

            var version = new LinterhubWrapper(context).LinterVersion(context.Linter, versionCmd).Trim();

            result.LinterName = context.Linter;
            result.Installed = version.Contains("Can\'t find " + context.Linter) ? "No" : "Yes";
            result.Version = ((string)result.Installed).Contains("No") ? "Unknown" : version;

            return JsonConvert.SerializeObject(result);
        }

        
    }
}