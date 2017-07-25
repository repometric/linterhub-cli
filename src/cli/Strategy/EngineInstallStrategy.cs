namespace Linterhub.Cli.Strategy
{
    using Runtime;
    using Core.Schema;
    using Core.Runtime;
    using Core.Factory;
    using System.Linq;

    /// <summary>
    /// The 'install engines' strategy logic.
    /// </summary>
    public class EngineInstallStrategy : IStrategy
    {
        /// <summary>
        /// Run strategy.
        /// </summary>
        /// <param name="locator">The service locator.</param>
        /// <returns>Run results (list of install results).</returns>
        public object Run(ServiceLocator locator)
        {
            var context = locator.Get<RunContext>();
            var projectConfig = locator.Get<LinterhubConfigSchema>();
            var engineFactory = locator.Get<IEngineFactory>();
            var installer = locator.Get<Installer>();
            var ensure = locator.Get<Ensure>();

            // Select engines from context or config
            context.Engines = context.Engines.Any() ? context.Engines : projectConfig.Engines.Select(x => x.Name).ToArray();

            ensure.EngineSpecified();
            ensure.EngineExists();

            var result = context.Engines.Select(engine => 
            {
                var specification = engineFactory.GetSpecification(engine);
                return installer.Install(specification);
            }).ToList();

            return result;
        }
    }
}