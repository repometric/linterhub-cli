namespace Linterhub.Cli.Strategy
{
    using Runtime;
    using Engine;

    public interface IStrategy
    {
        object Run(RunContext context, LinterFactory factory, LogManager log);
    }
}