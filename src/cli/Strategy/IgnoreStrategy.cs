namespace Linterhub.Cli.Strategy
{
    using System.Linq;
    using Runtime;
    using Engine;
    using Engine.Exceptions;

    public class IgnoreStrategy : IStrategy
    {
        public object Run(RunContext context, LinterFactory factory, LogManager log)
        {
            ProjectConfig.IgnoreRule rule = new ProjectConfig.IgnoreRule();
            var validationContext = context.ValidateContext(factory, log);
            if (context.File != null)
            {
                rule.FileName = context.File;
            }
            if (context.ExtraArgs.ContainsKey("line"))
            {
                rule.Line = int.Parse(context.ExtraArgs["line"]);
            }
            if (context.ExtraArgs.ContainsKey("error"))
            {
                rule.Error = context.ExtraArgs["error"];
            }

            if (validationContext.IsLinterSpecified)
            {
                var linter = validationContext.ProjectConfig.Linters.FirstOrDefault(x => x.Name == context.Linter);
                if (linter != null)
                {
                    if (factory.GetRecords().FirstOrDefault(x => x.Name == context.Linter) == null)
                    {
                        throw new LinterEngineException("Linter is not exist: " + context.Linter);
                    }
                    if(linter.Ignore.Where(x => x.Error == rule.Error && x.FileName == rule.FileName && x.Line == rule.Line).Count() == 0)
                        linter.Ignore.Add(rule);
                }
                else
                {
                    throw new LinterEngineException("Config for this linter is not exist in project file: " + context.Linter);
                }
            }
            else
            {
                if(validationContext.ProjectConfig.Ignore.Where(x => x.Error == rule.Error && x.FileName == rule.FileName && x.Line == rule.Line).Count() == 0)
                    validationContext.ProjectConfig.Ignore.Add(rule);
            }

            return context.SetProjectConfig(validationContext);
        }
    }
}