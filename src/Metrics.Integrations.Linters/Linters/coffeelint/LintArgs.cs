namespace Metrics.Integrations.Linters.coffeelint
{
    public class LintArgs : ILinterArgs
    {
        /// <summary>
        /// Tool path
        /// </summary>
        [Arg("", separator: "", order: int.MinValue)]
        public string ToolPath { get; set; }

        /// <summary>
        ///  built in reporter (default, csv, jslint, checkstyle, raw),
        ///  or module, or path to reporter file.
        /// </summary>
        [Arg("--reporter", order: 0)]
        public string ReportType { get; set; }

        /// <summary>
        /// Tested project path
        /// </summary>
        [Arg("", separator: "", order: int.MaxValue)]
        public string TestPath { get; set; }
    }
}