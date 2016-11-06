namespace Linterhub.Cli.Strategy
{
    using Newtonsoft.Json;
    using Linterhub.Cli.Runtime;
    using Linterhub.Engine;

    public class GenerateStrategy : IStrategy
    {
        public object Run(RunContext context, LinterEngine engine, LogManager log)
        {
            var args = engine.Factory.CreateArguments(context.Linter);
            return args;
        }
    }
}