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
        /// Get engine version.
        /// </summary>
        EngineVersion,
        /// <summary>
        /// Install linter.
        /// </summary>
        EngineInstall,
        /// <summary>
        /// Exclude objects from the analysis.
        /// </summary>
        Ignore,
        /// <summary>
        /// Fetche some useful engines
        /// </summary>
        FetchEngines
    }
}