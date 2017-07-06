namespace Linterhub.Cli.Strategy
{
    using System.Linq;
    using Core.Schema;
    using Runtime;
    using System.Collections.Generic;

    public class IgnoreStrategy : IStrategy
    {
        private bool exists(List<LinterhubConfigSchema.IgnoreType> list, LinterhubConfigSchema.IgnoreType element)
        {
            return list.Exists(x => x.Line == element.Line && x.Mask == element.Mask && x.RuleId == element.RuleId);
        }

        public object Run(ServiceLocator locator)
        {
            var context = locator.Get<RunContext>();
            var config = locator.Get<LinterhubConfigSchema>();
            var projectConfig = locator.Get<LinterhubConfigSchema>();
            var ensure = locator.Get<Ensure>();

            // Validate
            ensure.ProjectSpecified();


            var rule = new LinterhubConfigSchema.IgnoreType()
            {
                Mask = context.Path,
                Line = context.Line,
                RuleId = context.RuleId
            };

            if (context.Engines.Any())
            {
                // TODO: File could be mask
                // TODO: Rule for project > for file > for line. Avoid dublicates and improve logic
                foreach (var engine in context.Engines)
                {
                    var projectEngine = projectConfig.Engines.FirstOrDefault(x => x.Name == engine);
                    if (projectEngine == null)
                    {
                        projectEngine = new LinterhubConfigSchema.ConfigurationType
                        {
                            Name = engine
                        };
                        projectConfig.Engines.Add(projectEngine);
                    }

                    if (!exists(projectEngine.Ignore, rule))
                    {
                        projectEngine.Ignore.Add(rule);
                    }
                }
            }
            else
            {
                if (!exists(projectConfig.Ignore, rule))
                {
                    projectConfig.Ignore.Add(rule);
                }
            }

            context.SaveConfig = true;
            return projectConfig;
        }
    }
}