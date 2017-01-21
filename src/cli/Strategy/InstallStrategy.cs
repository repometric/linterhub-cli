namespace Linterhub.Cli.Strategy
{
    using Runtime;
    using Engine;
    using Linterhub.Engine.Exceptions;

    public class InstallStrategy : IStrategy
    {
        public object Run(RunContext context, LinterFactory factory, LogManager log)
        {
            if (string.IsNullOrEmpty(context.Linter))
            {
                throw new LinterEngineException("Linter is not specified: " + context.Linter);
            }

            var installCmd = string.Format("--mode engine:install --name {0} --log NO", context.Linter);

            new LinterhubWrapper(context).Run(installCmd);

            return new LinterVersionStrategy().Run(context, factory, log);
        }

        
    }
}