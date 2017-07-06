namespace Linterhub.Cli.Strategy
{
    using System.Linq;
    using Core.Factory;
    using Core.Schema;

    /// <summary>
    /// The 'catalog engine' strategy logic.
    /// </summary>
    public class CatalogStrategy : IStrategy
    {
        /// <summary>
        /// Run strategy.
        /// </summary>
        /// <param name="locator">The service locator.</param>
        /// <returns>Run results (list of engines).</returns>
        public object Run(ServiceLocator locator)
        {
            var factory = locator.Get<IEngineFactory>();
            var projectConfig = locator.Get<LinterhubConfigSchema>();
            
            // List all engines and detect active engines (active for the project)
            var engines = factory.GetSpecifications().Select(x => x.Schema).OrderBy(x => x.Name);
            var result = engines.Select(engine => 
            {
                engine.Active = projectConfig.Engines.Any(projectEngine => projectEngine.Name == engine.Name && (projectEngine.Active ?? false));
                return engine;
            });

            return result.OrderBy(x => x.Name).Select((x) => {
                if(x.Active == false)
                {
                    x.Active = null;
                }
                if(x.SuccessCode == 0)
                {
                    x.SuccessCode = null;
                }
                return x;
            });
        }
    }
}