namespace Linterhub.Cli.Strategy
{
    using Linterhub.Cli.Runtime;
    using Linterhub.Engine;

    public interface IStrategy
    {
        object Run(RunContext context, LinterEngine engine);
    }
}