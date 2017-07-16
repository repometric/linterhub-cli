namespace Linterhub.Cli.Strategy
{
    using System.Linq;
    using Runtime;
    using Core.Schema;

    /// <summary>
    /// The 'activate engine' strategy logic.
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
            ensure.EngineSpecified();
            ensure.EngineExists();
            ensure.ProjectSpecified();
            ensure.ArgumentSpecified(nameof(context.Activate), context.Activate);

            // Enumerate engines and activate/deactivate
            foreach (var engine in context.Engines)
            {
                var projectEngine = projectConfig.Engines.FirstOrDefault(x => x.Name == engine);
                if (projectEngine != null)
                {
                    projectEngine.Active = context.Activate;
                }
                else
                {
                    projectConfig.Engines.Add(new LinterhubConfigSchema.ConfigurationType
                    { 
                        Name = engine,
                        Active = context.Activate
                    });
                }
            }

            context.SaveConfig = true;
            return null;
        }
    }
}