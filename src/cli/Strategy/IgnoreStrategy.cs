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
            var config = locator.Get<LinterhubSchema>();
            var rule = new LinterhubSchema.IgnoreRule()
            {
                Path = context.Path,
                Line = context.Line,
                RuleId = context.RuleId
            };

            if (context.Linters.Any())
            {
                // TODO: File could be mask
                // TODO: Rule for project > for file > for line. Avoid dublicates and improve logic
                foreach (var linter in context.Linters)
                {
                    var linterConfig = config.Linters.FirstOrDefault(x => x.Name == linter);
                    if (linterConfig == null)
                    {
                        linterConfig = new LinterhubSchema.Linter
                        {
                            Name = linter
                        };
                        config.Linters.Add(linterConfig);
                    }

                    linterConfig.Ignore.Add(rule);
                }
            }
            else
            {
                config.Ignore.Add(rule);
            }

            context.SaveConfig = true;
            return null;
        }
    }
}