namespace Metrics.Integrations.Linters.pep8
{
    public class LintArgs : ILinterArgs
    {
        /// <summary>
        /// Tested project path
        /// </summary>
        public string TestPath { get; set; }

        /// <summary>
        /// Tested project path (in container)
        /// </summary>
        [Arg("", order: 0)]
        public string TestPathDocker { get; set; }

        /// <summary>
        /// Tool path
        /// </summary>
        [Arg("", order: int.MinValue)]
        public string ToolPath { get; set; }

        /// <summary>
        /// Set the error format [default|pylint|<custom>]
        /// </summary>
        [Arg("--format", separator: "=", order: 1)]
        public string ReportType { get; set; }

        /// <summary>
        /// Exclude files or directories which match these comma
        /// separated patterns(default: .svn, CVS,.bzr,.hg,.git)
        /// </summary>
        [Arg("--exclude", separator: "=", order: 1)]
        public string Exclude { get; set; }

        /// <summary>
        /// When parsing directories, only check filenames matching
        /// these comma separated patterns(default: *.py)
        /// </summary>
        [Arg("--filename", separator: "=", order: 1)]
        public string Filename { get; set; }

        /// <summary>
        /// Select errors and warnings (e.g. E,W6)
        /// http://pep8.readthedocs.io/en/release-1.7.x/intro.html#error-codes
        /// </summary>
        [Arg("--select", separator: "=", order: 1)]
        public string SelectErrors { get; set; }

        /// <summary>
        /// Skip errors and warnings (e.g. E4,W)
        /// http://pep8.readthedocs.io/en/release-1.7.x/intro.html#error-codes
        /// </summary>
        [Arg("--ignore", separator: "=", order: 1)]
        public string IgnoreErrors { get; set; }

        /// <summary>
        /// Set maximum allowed line length (default: 79)
        /// </summary>
        [Arg("--max-line-length", separator: "=", order: 1)]
        public int MaxLineLength { get; set; }

    }
}