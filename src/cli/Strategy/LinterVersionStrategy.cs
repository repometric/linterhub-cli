namespace Linterhub.Cli.Strategy
{
    using System.Linq;
    using Linterhub.Engine.Runtime;
    using Linterhub.Cli.Runtime;
    using Linterhub.Engine.Schema;
    using Linterhub.Engine.Factory;

    /// <summary>
    /// The 'linter version' strategy logic.
    /// </summary>
    public class LinterVersionStrategy : IStrategy
    {
        /// <summary>
        /// Run strategy.
        /// </summary>
        /// <param name="locator">The service locator.</param>
        /// <returns>Run results (list of linter versions).</returns>
        public object Run(ServiceLocator locator)
        {
            var ensure = locator.Get<Ensure>();
            var context = locator.Get<RunContext>();
            var projectConfig = locator.Get<LinterhubConfigSchema>();
            var linterFactory = locator.Get<ILinterFactory>();
            var installer = locator.Get<Installer>();

            // Check
            ensure.LinterSpecified();
            ensure.LinterExists(); 

            var linters = context.Linters.Any() ? context.Linters : projectConfig.Engines.Select(x => x.Name);

            return linters.Select(linter => 
            {
                var specification = linterFactory.GetSpecification(linter);
                return installer.IsInstalled(
                    specification.Schema.Requirements
                    .Where(x => x.Package == specification.Schema.Name)
                    .FirstOrDefault()
                );
            }).FirstOrDefault();
        }
    }
}