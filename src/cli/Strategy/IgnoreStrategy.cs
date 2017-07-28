namespace Linterhub.Cli.Strategy
{
    using System.Linq;
    using Core.Schema;
    using Core.Exceptions;
    using Runtime;
    using System.Collections.Generic;
    using System.IO;

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

            var Mask = "";

            if (context.Directory != null)
            {
                var relative = context.Directory
                    .Replace(context.Project, string.Empty)
                    .Replace(Path.GetFullPath(context.Project), string.Empty)
                    .TrimStart('/')
                    .TrimStart('\\')
                    .Replace("/", "\\");
                Mask = Path.Combine(relative, (context.File ?? string.Empty));
            }
            else
            {
                Mask = context.File;
            }

            var rule = new LinterhubConfigSchema.IgnoreType()
            {
                Mask = Mask,
                Line = context.Line,
                RuleId = context.RuleId
            };

            if(config == null)
            {
                throw new LinterhubException("Invalid project config", "Catch null while parsing project config", LinterhubException.ErrorCode.linterhubConfig);
            }

            if (context.Engines.Any())
            {
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