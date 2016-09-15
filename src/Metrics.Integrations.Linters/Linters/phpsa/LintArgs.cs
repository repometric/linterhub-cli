namespace Metrics.Integrations.Linters.Phpsa
{
    public class LintArgs : ILinterArgs
    {
        /// <summary>
        /// Report file
        /// </summary>
        [Arg("--report-json", separator: "=", order: 1)]
        public string OutputFile { get; set; }

        /// <summary>
        /// Increase the verbosity of messages: 1 for normal output,
        /// 2 for more verbose output and 3 for debug
        /// </summary>
        //[Arg("--verbose", separator: "=", order: 1)]
        //public int Verbose { get; set; }

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