namespace Metrics.Integrations.Linters.PhpAssumptions
{
    public class LintArgs : ILinterArgs
    {
        /// <summary>
        /// Report file path (default: phpa.xml)
        /// </summary>
        [Arg("-o", separator: " ", order: int.MaxValue)]
        public string OutputFile { get; set; } 

        /// <summary>
        /// Tool path
        /// </summary>
        [Arg("", order: int.MinValue)]
        public string ToolPath { get; set; }

        /// <summary>
        ///  Report format
        /// </summary>
        [Arg("-f", separator: " ", order: int.MaxValue)]
        public string OutputFormat { get; set; }

        /// <summary>
        /// Tested project path
        /// </summary>
        [Arg("", order: 0)]
        public string TestPath { get; set; }
    }
}