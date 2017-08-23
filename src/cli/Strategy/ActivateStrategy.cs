namespace Linterhub.Cli.Strategy
{
    using System.Linq;
    using Core.Schema;
    using Core.Managers;
    using Core.Factory;
    using Runtime;
    using Core.Runtime;
    using System.Collections.Generic;

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
            var installer = locator.Get<Installer>();

            context.Engines = context.Engines.Any() ? context.Engines : projectConfig.Engines.Select(x => x.Name).ToArray();
            string installationPath = null;
            var activate = context.Mode == RunMode.Activate;
            var result = new List<EngineVersionSchema.ResultType>();

            // Validate
            ensure.EngineSpecified();
            ensure.EngineExists();

            if(context.Locally || ensure.ProjectSpecifiedCheck())
            {
                ensure.ProjectSpecified();
                installationPath = context.Project;
            }

            // Enumerate engines and activate/deactivate
            foreach (var engine in context.Engines)
            {
                if(activate)
                {
                    var specification = engineFactory.GetSpecification(engine);
                    result.Add(installer.Install(specification, installationPath));
                }

                var projectEngine = projectConfig.Engines.FirstOrDefault(x => x.Name == engine);
                if (projectEngine != null)
                {
                    projectEngine.Active = activate;
                    projectEngine.Locally = installationPath != null;
                }
                else
                {
                    projectConfig.Engines.Add(new LinterhubConfigSchema.ConfigurationType
                    {
                        Name = engine,
                        Active = activate,
                        Locally = installationPath != null
                    });
                }

            }

            context.SaveConfig = true;
            return result;
        }
    }
}