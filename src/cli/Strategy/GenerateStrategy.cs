namespace Linterhub.Cli.Strategy
{
    using Runtime;
    using Engine;

    public class GenerateStrategy : IStrategy
    {
        public object Run(RunContext context, LinterFactory factory, LogManager log)
        {
            var args = factory.CreateArguments(context.Linter);
            return args;
        }
    }
}