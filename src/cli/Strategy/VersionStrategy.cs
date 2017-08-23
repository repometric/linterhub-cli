namespace Linterhub.Cli.Strategy
{
    using System;
    using System.Reflection;
    using Core.Runtime;
    using Runtime;
    using System.Linq;
    using Core.Schema;
    using Core.Factory;
    using Core.Managers;

    /// <summary>
    /// The 'version' strategy logic.
    /// </summary>
    public class VersionStrategy : IStrategy
    {
        /// <summary>
        /// Run strategy.
        /// </summary>
        /// <param name="locator">The service locator.</param>
        /// <returns>Run results (version string).</returns>
        public object Run(ServiceLocator locator)
        {
            // Engine version is not needed right now
            // var engineVersion = GetVersion(typeof(LinterSpecification));
            var context = locator.Get<RunContext>();
            var ensure = locator.Get<Ensure>();

            if (context.Engines.Length != 0 || ensure.ProjectSpecifiedCheck())
            {
                var projectConfig = locator.Get<LinterhubConfigSchema>();
                var engineFactory = locator.Get<IEngineFactory>();
                var installer = locator.Get<Installer>();
                var managerWrapper = locator.Get<ManagerWrapper>();

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
            else
            {
                var version = GetVersion(typeof(VersionStrategy)).Split('.');
                return string.Join(".", version.Take(version.Length - 1));
            }
        }

        private string GetVersion(Type type)
        {
            return type.GetTypeInfo().Assembly.GetName().Version.ToString();
        }
    }
}