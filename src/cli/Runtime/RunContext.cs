namespace Linterhub.Cli.Runtime
{
    using System.IO;

    /// <summary>
    /// Represents run context (runtime parameters).
    /// </summary>
    public class RunContext
    {
        #region Common arguments

        /// <summary>
        /// Gets or sets the run mode.
        /// </summary>
        public RunMode Mode { get; set; }

        /// <summary>
        /// Gets or sets the path to the project for analysis.
        /// </summary>
        public string Project { get; set; }

        /// <summary>
        /// Gets or sets the path to the file for analysis.
        /// </summary>
        public string File { get; set; }

        /// <summary>
        /// Gets or sets the path to the folder for analysis.
        /// </summary>
        public string Directory { get; set; }

        /// <summary>
        /// Gets or sets the path to the project configuration.
        /// </summary>
        public string ProjectConfig { get; set; }

        /// <summary>
        /// Gets or sets the path to the linterhub folder.
        /// </summary>
        public string Linterhub { get; set; }

        /// <summary>
        /// Gets or sets the path to the platfrom configuration.
        /// </summary>
        public string PlatformConfig { get; set; }

        /// <summary>
        /// Gets or sets the list of engines for the analysis.
        /// </summary>
        public string[] Engines { get; set; }

        #endregion

        #region More specific arguments

        /// <summary>
        /// Gets or sets the value indicationg whether to activate engine.
        /// </summary>
        public bool? Activate { get; set; }

        /// <summary>
        /// Gets or sets the stdin.
        /// </summary>
        public Stream Input { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether stdin is available.
        /// </summary>
        public bool InputAwailable { get; set; }

        /// <summary>
        /// Gets or sets the path to ignore. 
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the line to ignore.
        /// </summary>
        public int? Line { get; set; }

        /// <summary>
        /// Gets or sets the rule to ignore.
        /// </summary>
        public string RuleId { get; set; }

        #endregion

        /// <summary>
        /// Gets or sets the list of key filters for stdout.
        /// </summary>
        public string[] Keys { get; set; }
        
        /// <summary>
        /// Gets or sets the list of value filters for stdout.
        /// </summary>
        public string[] Filters { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether to save project config.
        /// </summary>
        public bool SaveConfig { get; set; }

        /// <summary>
        /// Initializes a new instance of <seealso cref="RunContext"/> class.
        /// </summary>
        /// <param name="mode">The run mode.</param>
        public RunContext(RunMode mode = RunMode.Help)
        {
            Mode = mode;
            Engines = new string[0];
            Keys = new string[0];
            Filters = new string[0];
        }
    }
}