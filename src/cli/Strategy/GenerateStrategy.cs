namespace Linterhub.Cli.Strategy
{
    using Runtime;
    using Engine;

    public class GenerateStrategy : IStrategy
    {
        public object Run(RunContext context, LinterEngine engine, LogManager log)
        {
            var args = engine.Factory.CreateArguments(context.Linter);
            return args;
        }
    }
}