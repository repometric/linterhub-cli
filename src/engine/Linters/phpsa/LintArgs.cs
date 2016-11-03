namespace Linterhub.Engine.Linters.Phpsa
{
    public class LintArgs : ILinterArgs
    {
        /// <summary>
        /// Report file
        /// </summary>
        [Arg("--report-json", separator: "=", order: 1)]
        public string OutputFile { get; set; }

        /// <summary>
        /// Increase the verbosity of messages: -v for normal output,
        /// -vv for more verbose output and -vvv for debug
        /// </summary>
        [Arg("", separator: "", order: 1)]
        public string Verbose { get; set; }

        /// <summary>
        /// Tool path
        /// </summary>
        [Arg("", order: int.MinValue)]
        public string ToolPath { get; set; }

        /// <summary>
        /// Command (by default: check)
        /// </summary>
        [Arg("", order: -1)]
        public string Command { get; set; }

        /// <summary>
        /// Tested project path
        /// </summary>
        [Arg("", order: int.MaxValue)]
        public string TestPath { get; set; }
    }
}