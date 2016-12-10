namespace Linterhub.Engine.Linters.Phpmetrics
{
    public class LintArgs : ILinterArgs
    {
        /// <summary>
        /// Tested project path
        /// </summary>
        [Arg("", order: int.MaxValue)]
        public string TestPath { get; set; }

        /// <summary>
        /// Tool path
        /// </summary>
        [Arg("", order: int.MinValue)]
        public string ToolPath { get; set; }

        /// <summary>
        /// Output File
        /// </summary>
        [Arg("--report-json", separator: "=", order: 1)]
        public string OutputFile { get; set; }

        [Arg("--extensions", separator: "=", order: 1)]
        public string Extensions { get; set; }

        [Arg("--excluded-dirs ", separator: "=", order: 1)]
        public string ExcludedDirectories { get; set; }

        /// <summary>
        /// Tested project path (in container)
        /// </summary>
        [Arg("", order: int.MaxValue)]
        public string TestPathDocker { get; set; }
    }
}