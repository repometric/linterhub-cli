namespace Linterhub.Cli.Strategy
{
    using Linterhub.Cli.Runtime;
    using Linterhub.Engine.Schema;
    using Linterhub.Engine.Runtime;
    using Linterhub.Engine.Factory;
    using System.Linq;

    /// <summary>
    /// The 'install linters' strategy logic.
    /// </summary>
    public class LinterInstallStrategy : IStrategy
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
            var linterFactory = locator.Get<ILinterFactory>();
            var installer = locator.Get<Installer>();

            // Select linters from context or config
            var linters = context.Linters.Any() ? context.Linters : projectConfig.Engines.Select(x => x.Name);

            var result = linters.Select(linter => 
            {
                var specification = linterFactory.GetSpecification(linter);
                return installer.Install(specification);
            }).ToList();

            return result;
        }
    }
}