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
            var validationContext = context.ValidateContext(factory, log);
            if (!validationContext.IsLinterSpecified)
            {
                throw new LinterEngineException("Linter is not specified: " + context.Linter);
            }
            if(factory.GetRecords().FirstOrDefault(x => x.Name == context.Linter) == null)
            {
                throw new LinterEngineException("Linter is not exist: " + context.Linter);
            }

            var linter = validationContext.ProjectConfig.Linters.FirstOrDefault(x => x.Name == context.Linter);
            if (linter != null)
            {
                linter.Active = context.Activate;
            }
            else
            {
                validationContext.ProjectConfig.Linters.Add(new ProjectConfig.Linter 
                {
                    Name = context.Linter,
                    Active = context.Activate,
                    Config = new object()
                    //Command = factory.BuildCommand(context.Linter, "", "", ArgMode.Folder)
                });
            }

            return context.SetProjectConfig(validationContext);
        }
    }
}