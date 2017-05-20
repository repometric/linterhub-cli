namespace Linterhub.Cli.Strategy
{
    using System.Linq;
    using Linterhub.Engine.Schema;
    using Linterhub.Cli.Runtime;

    public class IgnoreStrategy : IStrategy
    {
        public object Run(ServiceLocator locator)
        {
            var context = locator.Get<RunContext>();
            var config = locator.Get<LinterhubConfigSchema>();
            var rule = new LinterhubConfigSchema.IgnoreType()
            {
                Mask = context.Path,
                Line = context.Line,
                RuleId = context.RuleId
            };

            if (context.Linters.Any())
            {
                // TODO: File could be mask
                // TODO: Rule for project > for file > for line. Avoid dublicates and improve logic
                foreach (var linter in context.Linters)
                {
                    var linterConfig = config.Engines.FirstOrDefault(x => x.Name == linter);
                    if (linterConfig == null)
                    {
                        linterConfig = new LinterhubConfigSchema.ConfigurationType
                        {
                            Name = linter
                        };
                        config.Engines.Append(linterConfig);
                    }

                    linterConfig.Ignore.Append(rule);
                }
            }
            else
            {
                config.Ignore.Append(rule);
            }

            context.SaveConfig = true;
            return null;
        }
    }
}