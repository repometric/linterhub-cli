namespace Linterhub.Engine.Linters.Phpcs
{
    public class LintArgs : ILinterArgs
    {
        /// <summary>
        /// Report type. Default: JSON
        /// </summary>
        [Arg("--report", separator: "=", order: 1)]
        public string ReportType { get; set; } 

        /// <summary>
        /// Extensions to check. By default .inc, .php, .js and .css
        /// </summary>
        [Arg("--extensions", separator: "=", order: 1)]
        public string Extensions { get; set; } 

        /// <summary>
        /// Files and directories to ignore
        /// </summary>
        [Arg("--ignore", separator: "=", order: 1)]
        public string Ignore { get; set; } 

        /// <summary>
        /// Standard to check (For example PEAR)
        /// </summary>
        [Arg("--standard", separator: "=", order: 1)]
        public string Standard { get; set; }

        /// <summary>
        /// List of sniffs to use. By default use all
        /// </summary>
        [Arg("--sniffs", separator: "=", order: 1)]
        public string Sniffs { get; set; } 

        /// <summary>
        /// List of sniffs to ignore
        /// </summary>
        [Arg("--exclude", separator: "=", order: 1)]
        public string ExcludeSniffs { get; set; } 

        /// <summary>
        /// Severity. For example if you set 3, phpcs will hide all errors and warnings with a severity less than 3
        /// </summary>
        [Arg("--severity", separator: "=", order: 1)]
        public string Severity { get; set; } 

        /// <summary>
        /// Error Severity
        /// </summary>
        [Arg("--error-severity", separator: "=", order: 1)]
        public string ErrorSeverity { get; set; } 

        /// <summary>
        /// Warning Severity
        /// </summary>
        [Arg("--warning-severity", separator: "=", order: 1)]
        public string WarningSeverity { get; set; }

        /// <summary>
        /// Set encoding (by default ISO-8859-1)
        /// </summary>
        [Arg("--encoding", separator: "=", order: 1)]
        public string Encoding { get; set; }

        /// <summary>
        /// Include one or more custom bootstrap files before beginning the run
        /// </summary>
        [Arg("--bootstrap", separator: "=", order: 1)]
        public string Bootstrap { get; set; } 

        /// <summary>
        /// Tool path
        /// </summary>
        [Arg("", order: int.MinValue)]
        public string ToolPath { get; set; }

        /// <summary>
        /// Tested project path
        /// </summary>
        public string TestPath { get; set; }

        /// <summary>
        /// Tested project path (in container)
        /// </summary>
        [Arg("", order: int.MaxValue)]
        public string TestPathDocker { get; set; }
    }
}