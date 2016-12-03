namespace Linterhub.Engine.Linters.PhpAssumptions
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
        [Arg("phpa", false, order: int.MinValue)]
        public string PhpA { get; set; }

        /// <summary>
        ///  Report format
        /// </summary>
        [Arg("-f", separator: " ", order: int.MaxValue)]
        public string ReportType { get; set; }

        /// <summary>
        /// Tested project path
        /// </summary>
        [Arg("", order: 0)]
        public string TestPath { get; set; }
    }
}