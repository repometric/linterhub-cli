namespace Linterhub.Cli.Runtime
{
    /// <summary>
    /// Represents run mode for CLI.
    /// </summary>
    public enum RunMode
    {
        /// <summary>
        /// Activate engine for project.
        /// </summary>
        Activate,
        /// <summary>
        /// Deactivate engine for project.
        /// </summary>
        Deactivate,
        /// <summary>
        /// Analyze project.
        /// </summary>
        Analyze,
        /// <summary>
        /// Analyze stdin.
        /// </summary>
        AnalyzeStdin,
        /// <summary>
        /// List available engines.
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
        /// Exclude objects from the analysis.
        /// </summary>
        Ignore,
        /// <summary>
        /// Fetche some useful engines for project
        /// </summary>
        Fetch
    }
}