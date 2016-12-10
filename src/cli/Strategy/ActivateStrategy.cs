namespace Linterhub.Cli.Strategy
{
    using System.Linq;
    using Runtime;
    using Engine;
    using Engine.Exceptions;

    public class ActivateStrategy : IStrategy
    {
        public object Run(RunContext context, LinterFactory factory, LogManager log)
        {
            if (string.IsNullOrEmpty(context.Linter))
            {
                throw new LinterEngineException("Linter is not specified: " + context.Linter);
            }

            var extConfig = context.GetProjectConfig();
            var linter = extConfig.Linters.FirstOrDefault(x => x.Name == context.Linter);
            if (linter != null)
            {
                linter.Active = context.Activate ? (bool?)null : false;
            }
            else
            {
                extConfig.Linters.Add(new ProjectConfig.Linter 
                {
                    Name = context.Linter,
                    Active = context.Activate,
                    Command = factory.BuildCommand(context.Linter)
                });
            }

            return context.SetProjectConfig(extConfig);
        }
    }
}