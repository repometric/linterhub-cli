namespace Linterhub.Engine.Linters.csslint
{
    public class LintArgs : ILinterArgs
    {

        public LintArgs()
        {
            ToolPath = "csslint";
            ReportType = "json";
            TestPathDocker = "./";
        }

        /// <summary>
        /// Tool path
        /// </summary>
        [Arg("", order: int.MinValue)]
        public string ToolPath { get; set; }

        /// <summary>
        /// Indicate which format to use for output.
        /// </summary>
        [Arg("--format", separator: "=", order: 1)]
        public string ReportType { get; set; }

        /// <summary>
        /// Indicate which rules to include as errors.
        /// </summary>
        [Arg("--errors", separator: "=", order: 1)]
        public string Errors { get; set; }

        /// <summary>
        /// Indicate which rules to include as warnings.
        /// </summary>
        [Arg("--warnings", separator: "=", order: 1)]
        public string Warnings { get; set; }

        /// <summary>
        /// Indicate which files/directories to exclude from being linted.
        /// </summary>
        [Arg("--exclude-list", separator: "=", order: 1)]
        public string Exclude { get; set; }

        /// <summary>
        /// Tested project path (in container)
        /// </summary>
        [Arg("", order: int.MaxValue)]
        public string TestPathDocker { get; set; }

    }
}