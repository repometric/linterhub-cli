namespace Linterhub.Engine.Linters.Phpmd
{
    public class LintArgs : ILinterArgs
    {
        /// <summary>
        /// Report type. Default: XML
        /// </summary>
        [Arg("", separator: "", order: 2)]
        public string ReportType { get; set; } 

        /// <summary>
        /// Tool path
        /// </summary>
        [Arg("", order: int.MinValue)]
        public string ToolPath { get; set; }

        /// <summary>
        /// Rulesets. Available: cleancode, codesize, controversial, design, naming, unusedcode
        /// Also u can choose a file
        /// </summary>
        [Arg("", order: int.MaxValue-1)]
        public string Rulesets { get; set; }

        /// <summary>
        /// The rule priority threshold; rules with lower priority than they will not be used
        /// </summary>
        [Arg("--minimumpriority", order: int.MaxValue)]
        public string MinimumPriority { get; set; }

        /// <summary>
        /// Sends the report output to the specified file, instead of the default output target STDOUT
        /// </summary>
        [Arg("--reportfile", order: int.MaxValue)]
        public string OutputFile { get; set; }

        /// <summary>
        /// Comma-separated string of valid source code filename extensions, e.g. php,phtml
        /// </summary>
        [Arg("--suffixes", order: int.MaxValue)]
        public string Suffixes { get; set; }

        /// <summary>
        /// Comma-separated string of patterns that are used to ignore directories
        /// </summary>
        [Arg("--exclude", order: int.MaxValue)]
        public string Exclude { get; set; }

        /// <summary>
        /// Also report those nodes with a @SuppressWarnings annotation
        /// </summary>
        [Arg("--strict", order: int.MaxValue)]
        public string Strict { get; set; }

        /// <summary>
        /// Tested project path
        /// </summary>
        [Arg("", order: 1)]
        public string TestPath { get; set; }

        /// <summary>
        /// Tested project path (in container)
        /// </summary>
        [Arg("", order: int.MaxValue)]
        public string TestPathDocker { get; set; }
    }
}