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
            var linterRunner = locator.Get<LinterWrapper>();
            var contextFactory = locator.Get<LinterContextFactory>();
            var installer = locator.Get<Installer>();

            // Check
            ensure.LinterSpecified();
            ensure.LinterExists();

            // Enumerate linters and get versions
            var contexts = contextFactory.GetContexts(context.Linters, context.Project, projectConfig);
            var results = 
                from linterContext in contexts
                let schema = linterContext.Specification.Schema
                let installed = installer.IsInstalled(linterContext.Specification.Schema.Requirements.Where(x => x.Package == linterContext.Specification.Schema.Name).FirstOrDefault())
                let version = installed.Installed ? linterRunner.RunVersion(linterContext) : null
                select new
                {
                    name = schema.Name,
                    version = version,
                    message = !installed.Installed ? $"Engine '{schema.Name}' is not installed" : null
                };

            var result = results.ToList();
            return result;
        }
    }
}