namespace Linterhub.Cli.Strategy
{
    using System.Linq;
    using Core.Schema;
    using Runtime;
    using System.Collections.Generic;

    public class IgnoreStrategy : IStrategy
    {
        private bool Exists(List<LinterhubConfigSchema.IgnoreType> list, LinterhubConfigSchema.IgnoreType element)
        {
            return list.Exists(x => x.Line == element.Line && x.Mask == element.Mask && x.RuleId == element.RuleId);
        }

        public object Run(ServiceLocator locator)
        {
            var context = locator.Get<RunContext>();
            var config = locator.Get<LinterhubConfigSchema>();
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
                    var projectEngine = config.Engines.FirstOrDefault(x => x.Name == engine);
                    if (projectEngine == null)
                    {
                        projectEngine = new LinterhubConfigSchema.ConfigurationType
                        {
                            Name = engine
                        };
                        config.Engines.Add(projectEngine);
                    }

                    if (!Exists(projectEngine.Ignore, rule))
                    {
                        projectEngine.Ignore.Add(rule);
                    }
                }
            }
            else
            {
                if (!Exists(config.Ignore, rule))
                {
                    config.Ignore.Add(rule);
                }
            }

            context.SaveConfig = true;
            return null;
        }
    }
}