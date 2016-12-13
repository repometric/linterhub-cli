namespace Linterhub.Engine.Linters.htmlhint
{
    public class LintArgs : ILinterArgs
    {

        public LintArgs()
        {
            HtmlHint = true;
            ReportType = "json";
        }

        /// <summary>
        /// Tested project path
        /// </summary>
        [ArgPath]
        public string Path { get; set; }

        /// <summary>
        /// Report type (checkstyle,compact,json,junit,markdown,unix)
        /// </summary>
        [Arg("--format", order: 1)]
        public string ReportType { get; set; }

        /// <summary>
        /// Set all of the rules available
        /// </summary>
        [Arg("--rules", order: 1)]
        public string Rules { get; set; }

        /// <summary>
        /// Add pattern to exclude matches
        /// </summary>
        [Arg("--ignore", order: 1)]
        public string Ignore { get; set; }

        /// <summary>
        /// Load custom rules from file or folder
        /// </summary>
        [Arg("--rulesdir", order: 1)]
        public string RulesDir { get; set; }

        /// <summary>
        /// Tool path
        /// </summary>
        [Arg("htmlhint", false, order: int.MinValue)]
        public bool HtmlHint { get; set; }

    }
}