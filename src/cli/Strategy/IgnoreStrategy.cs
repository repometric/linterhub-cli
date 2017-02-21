namespace Linterhub.Cli.Strategy
{
    using System.Linq;
    using Runtime;
    using Engine;
    using Engine.Exceptions;
    using System;
    using System.Collections.Generic;

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

            bool is_add = true;

            if (context.ExtraArgs.ContainsKey("add"))
            {
                is_add = Convert.ToBoolean(context.ExtraArgs["add"]);
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
                    if (findRule(linter.Ignore, rule) == null)
                    {
                        if (is_add)
                        {
                            linter.Ignore.Add(rule);
                        }
                    }
                    else
                    {
                        if (!is_add)
                        {
                            linter.Ignore.Remove(findRule(linter.Ignore, rule));
                        }
                    }
                }
                else
                {
                    throw new LinterEngineException("Config for this linter is not exist in project file: " + context.Linter);
                }
            }
            else
            {
                if (findRule(validationContext.ProjectConfig.Ignore, rule) == null)
                {
                    if (is_add)
                    {
                        validationContext.ProjectConfig.Ignore.Add(rule);
                    }
                }
                else
                {
                    if (!is_add)
                    {
                        validationContext.ProjectConfig.Ignore.Remove(findRule(validationContext.ProjectConfig.Ignore, rule));
                    }
                }
            }

            return context.SetProjectConfig(validationContext);
        }

        private ProjectConfig.IgnoreRule findRule(List<ProjectConfig.IgnoreRule> list, ProjectConfig.IgnoreRule rule)
        {
            return list.Where(x => x.Error == rule.Error && x.FileName == rule.FileName && x.Line == rule.Line).FirstOrDefault();
        }
    }
}