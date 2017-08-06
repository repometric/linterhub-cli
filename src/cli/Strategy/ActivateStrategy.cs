namespace Linterhub.Cli.Strategy
{
    using System.Linq;
    using Runtime;
    using Core.Schema;
    using Core.Managers;
    using Core.Factory;

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
            var managerWrapper = locator.Get<ManagerWrapper>();
            var engineFactory = locator.Get<IEngineFactory>();

            // Validate
            ensure.EngineSpecified();
            ensure.EngineExists();
            ensure.ProjectSpecified();
            ensure.ArgumentSpecified(nameof(context.Activate), context.Activate);

            var activate = context.Activate ?? true;

            // Enumerate engines and activate/deactivate
            foreach (var engine in context.Engines)
            {
                var manager = managerWrapper.get(engineFactory.GetSpecification(engine).Schema.Requirements.First().Manager);
                var installationPath = context.Locally ? context.Project : null;

                if (activate || !manager.CheckInstallation(engine, installationPath).Installed)
                {
                    var installResult = manager.Install(engine, installationPath);

                    if(!installResult.Installed)
                    {
                        return installResult;
                    }
                }

                var projectEngine = projectConfig.Engines.FirstOrDefault(x => x.Name == engine);
                if (projectEngine != null)
                {
                    projectEngine.Active = activate;
                }
                else
                {
                    projectConfig.Engines.Add(new LinterhubConfigSchema.ConfigurationType
                    { 
                        Name = engine,
                        Active = activate
                    });
                }
            }

            context.SaveConfig = true;
            return null;
        }
    }
}