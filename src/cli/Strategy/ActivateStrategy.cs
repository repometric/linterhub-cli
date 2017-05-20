namespace Linterhub.Cli.Strategy
{
    using System.Linq;
    using Linterhub.Cli.Runtime;
    using Linterhub.Engine.Schema;

    /// <summary>
    /// The 'activate linter' strategy logic.
    /// </summary>
    public class ActivateStrategy : IStrategy
    {
        /// <summary>
        /// Run strategy.
        /// </summary>
        /// <param name="locator">The service locator.</param>
        /// <returns>Run results (null).</returns>
        public object Run(ServiceLocator locator)
        {
            var ensure = locator.Get<Ensure>();
            var context = locator.Get<RunContext>();
            var projectConfig = locator.Get<LinterhubConfigSchema>();

            // Validate
            ensure.LinterSpecified();
            ensure.LinterExists();
            ensure.ArgumentSpecified(nameof(context.Activate), context.Activate);

            // Enumerate linters and activate/deactivate
            foreach (var linter in context.Linters)
            {
                var projectLinter = projectConfig.Engines.FirstOrDefault(x => x.Name == linter);
                if (projectLinter != null)
                {
                    projectLinter.Active = context.Activate;
                }
                else
                {
                    projectConfig.Engines.Append(new LinterhubConfigSchema.ConfigurationType
                    {
                        Name = linter
                    });
                }
            }

            context.SaveConfig = true;
            return null;
        }
    }
}