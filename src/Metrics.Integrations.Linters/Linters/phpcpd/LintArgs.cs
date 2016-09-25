namespace Metrics.Integrations.Linters.Phpcpd
{
    public class LintArgs : ILinterArgs
    {
        /// <summary>
        /// Report file path
        /// </summary>
        [Arg("--log-pmd", separator: "=", order: int.MaxValue)]
        public string OutputFile { get; set; } 

        /// <summary>
        /// Tool path
        /// </summary>
        [Arg("", order: int.MinValue)]
        public string ToolPath { get; set; }

        /// <summary>
        ///  Exclude a directory from code analysis (must be relative to source) (multiple values allowed)
        /// </summary>
        [Arg("--exclude", separator: "=", order: int.MaxValue)]
        public string Exclude { get; set; }

        /// <summary>
        ///  A comma-separated list of file names to exclude
        /// </summary>
        [Arg("--names-exclude", separator: "=", order: int.MaxValue)]
        public string NamesExclude { get; set; }

        /// <summary>
        ///  A comma-separated list of file names to check [default: ["*.php"]]
        /// </summary>
        [Arg("--names", separator: "=", order: int.MaxValue)]
        public string NamesToCheck { get; set; }

        /// <summary>
        ///  Minimum number of identical lines [default: 5]
        /// </summary>
        [Arg("--min-lines", separator: "=", order: int.MaxValue)]
        public string MinLines { get; set; }

        /// <summary>
        ///  Minimum number of identical tokens [default: 70]
        /// </summary>
        [Arg("--min-tokens", separator: "=", order: int.MaxValue)]
        public string MinTokens { get; set; }

        /// <summary>
        /// Tested project path
        /// </summary>
        [Arg("", order: 0)]
        public string TestPath { get; set; }
    }
}