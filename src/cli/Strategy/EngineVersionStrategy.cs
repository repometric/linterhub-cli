namespace Linterhub.Cli.Strategy
{
    using System.Linq;
    using Core.Runtime;
    using Runtime;
    using Core.Schema;
    using Core.Factory;
    using Core.Managers;
    using System.IO;

    /// <summary>
    /// The 'engine version' strategy logic.
    /// </summary>
    public class EngineVersionStrategy : IStrategy
    {
        /// <summary>
        /// Run strategy.
        /// </summary>
        /// <param name="locator">The service locator.</param>
        /// <returns>Run results (list of engines versions).</returns>
        public object Run(ServiceLocator locator)
        {
            var ensure = locator.Get<Ensure>();
            var context = locator.Get<RunContext>();
            var projectConfig = locator.Get<LinterhubConfigSchema>();
            var engineFactory = locator.Get<IEngineFactory>();
            var installer = locator.Get<Installer>();
            var managerWrapper = locator.Get<ManagerWrapper>();

            // Check
            ensure.EngineSpecified();
            ensure.EngineExists(); 

            var engines = context.Engines.Any() ? context.Engines : projectConfig.Engines.Select(x => x.Name);

            string installationPath = null;

            if (context.Locally)
            {
                ensure.ProjectSpecified();
                installationPath = context.Project;
            }

            return engines.Select(engine => 
            {
                var specification = engineFactory.GetSpecification(engine);

                var manager = managerWrapper.get(specification.Schema.Requirements
                    .Where(x => x.Package == specification.Schema.Name)
                    .FirstOrDefault().Manager);

                return manager.CheckInstallation(specification.Schema.Name, installationPath);
            });
        }
    }
}