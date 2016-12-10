namespace Linterhub.Engine.Linters.prospector
{
    public class LintArgs : ILinterArgs
    {

        public LintArgs()
        {
            ToolPath = "prospector";
            ReportType = "json";
        }

        /// <summary>
        /// Tool path
        /// </summary>
        [Arg("", order: int.MinValue)]
        public string ToolPath { get; set; }

        /// <summary>
        /// Indicate which format to use for output.
        /// </summary>
        [Arg("-o", separator: "=", order: int.MaxValue)]
        public string ReportType { get; set; }

        /// <summary>
        /// Tested project path (in container)
        /// </summary>
        [Arg("", order: 0)]
        public string TestPath { get; set; }

    }
}