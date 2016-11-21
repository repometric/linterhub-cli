namespace Linterhub.Cli.Strategy
{
    using System.Reflection;
    using Runtime;
    using Engine;

    public class VersionStrategy : IStrategy
    {
        public object Run(RunContext context, LinterEngine engine, LogManager log)
        {
            var linterhubVersion = new LinterhubWrapper(context, engine).Version().Trim();
            var engineVersion = typeof(LinterEngine).GetTypeInfo().Assembly.GetName().Version.ToString();
            var cliVersion = typeof(Program).GetTypeInfo().Assembly.GetName().Version.ToString();
            return 
                $"Linterhub: {linterhubVersion}\nEngine: {engineVersion}\nCLI: {cliVersion}";
        }
    }
}