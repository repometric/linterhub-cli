namespace Linterhub.Cli.Strategy
{
    using System.Reflection;
    using Runtime;
    using Engine;

    public class VersionStrategy : IStrategy
    {
        public object Run(RunContext context, LinterFactory factory, LogManager log)
        {
            var linterhubVersion = new LinterhubWrapper(context, factory).Version().Trim();
            var engineVersion = typeof(LinterFactory).GetTypeInfo().Assembly.GetName().Version.ToString();
            var cliVersion = typeof(Program).GetTypeInfo().Assembly.GetName().Version.ToString();
            return 
                $"Linterhub: {linterhubVersion}\nEngine: {engineVersion}\nCLI: {cliVersion}";
        }
    }
}