namespace Linterhub.Cli.Strategy
{
    using System;
    using System.Reflection;
    using Core.Schema;
    using System.Linq;

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
            var version = GetVersion(typeof(Program)).Split('.');
            return string.Join(".", version.Take(version.Length - 1));
        }

        private string GetVersion(Type type)
        {
            return type.GetTypeInfo().Assembly.GetName().Version.ToString();
        }
    }
}