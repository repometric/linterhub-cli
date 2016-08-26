using System;
using Metrics.Integrations.Linters;

namespace Metrics.Integrations.Linters.Phpcs
{
    public class LintArgs : ILinterArgs
    {
        /// <summary>
        /// Report type. Default: JSON
        /// </summary>
        [Arg("--report", valueSeparator: "=", order: 1)]
        public string ReportType { get; set; } 

        /// <summary>
        /// Extensions to check. By default .inc, .php, .js and .css
        /// </summary>
        [Arg("--extensions", valueSeparator: "=", order: 1)]
        public string Extensions { get; set; } 

        /// <summary>
        /// Files and directories to ignore
        /// </summary>
        [Arg("--ignore", valueSeparator: "=", order: 1)]
        public string Ignore { get; set; } 

        /// <summary>
        /// Standard to check (For example PEAR)
        /// </summary>
        [Arg("--standard", valueSeparator: "=", order: 1)]
        public string Standard { get; set; }

        /// <summary>
        /// List of sniffs to use. By default use all
        /// </summary>
        [Arg("--sniffs", valueSeparator: "=", order: 1)]
        public string Sniffs { get; set; } 

        /// <summary>
        /// List of sniffs to ignore
        /// </summary>
        [Arg("--exclude", valueSeparator: "=", order: 1)]
        public string ExcludeSniffs { get; set; } 

        /// <summary>
        /// Severity. For example if you set 3, phpcs will hide all errors and warnings with a severity less than 3
        /// </summary>
        [Arg("--severity", valueSeparator: "=", order: 1)]
        public string Severity { get; set; } 

        /// <summary>
        /// Error Severity
        /// </summary>
        [Arg("--error-severity", valueSeparator: "=", order: 1)]
        public string ErrorSeverity { get; set; } 

        /// <summary>
        /// Warning Severity
        /// </summary>
        [Arg("--warning-severity", valueSeparator: "=", order: 1)]
        public string WarningSeverity { get; set; }

        /// <summary>
        /// Set encoding (by default ISO-8859-1)
        /// </summary>
        [Arg("--encoding", valueSeparator: "=", order: 1)]
        public string Encoding { get; set; }

        /// <summary>
        /// Include one or more custom bootstrap files before beginning the run
        /// </summary>
        [Arg("--bootstrap", valueSeparator: "=", order: 1)]
        public string Bootstrap { get; set; } 
 

        /// <summary>
        /// Tool path
        /// </summary>
        [Arg("", order: int.MinValue)]
        public string ToolPath { get; set; }

        /// <summary>
        /// Tested project path
        /// </summary>
        [Arg("", order: int.MaxValue)]
        public string TestPath { get; set; }
    }
}