namespace Linterhub.Cli.Runtime
{
    /// <summary>
    /// Represents run mode for CLI.
    /// </summary>
    public enum RunMode
    {
        /// <summary>
        /// Activate linter for project.
        /// </summary>
        Activate,
        /// <summary>
        /// Analyze project.
        /// </summary>
        Analyze,
        /// <summary>
        /// List available linters.
        /// </summary>
        Catalog,
        /// <summary>
        /// Get app version.
        /// </summary>
        Version,
        /// <summary>
        /// Get app help.
        /// </summary>
        Help,
        /// <summary>
        /// Get linter version.
        /// </summary>
        LinterVersion,
        /// <summary>
        /// Install linter.
        /// </summary>
        LinterInstall,
        /// <summary>
        /// Exclude objects from the analysis.
        /// </summary>
        Ignore
    }
}