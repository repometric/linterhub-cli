namespace Linterhub.Cli.Strategy
{
    using Runtime;
    using Engine;
    using Linterhub.Engine.Exceptions;

    public class LinterVersionStrategy : IStrategy
    {
        public object Run(RunContext context, LinterFactory factory, LogManager log)
        {
            if (string.IsNullOrEmpty(context.Linter))
            {
                throw new LinterEngineException("Linter is not specified: " + context.Linter);
            }
            var result = "";
            var versionCmd = factory.BuildVersionCommand(context.Linter);
            //System.Console.WriteLine(versionCmd);

            var version = string.IsNullOrEmpty(versionCmd) ? "Unknown" : new LinterhubWrapper(context).LinterVersion(context.Linter, versionCmd).Trim();
            result += $"\n{context.Linter}: {version}";

            return result;
        }
    }
}