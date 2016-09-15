namespace Metrics.Integrations.Linters.Phpsa
{
    public class LintArgs : ILinterArgs
    {
        /// <summary>
        /// Report file
        /// </summary>
        [Arg("--report-json", separator: "=", order: int.MaxValue)]
        public string OutputFile { get; set; }

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
        [Arg("", order: 0)]
        public string TestPath { get; set; }
    }
}