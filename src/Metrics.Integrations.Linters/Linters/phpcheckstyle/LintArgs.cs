namespace Metrics.Integrations.Linters.phpcheckstyle
{
    using System.Collections.Generic;

    public class LintArgs : ILinterArgs
    {
        /// <summary>
        /// Report file path
        /// </summary>
        public string OutputFile {
            get
            {
                return OutputDir + "style-report.xml";
            }
        }

        [Arg("--outdir", order: int.MaxValue)]
        public string OutputDir { get; set; }

        /// <summary>
        /// Tool path
        /// </summary>
        [Arg("", order: int.MinValue)]
        public string ToolPath { get; set; }

        /// <summary>
        ///  Output format (html/text/xml/xml_console/console/html_console). Defaults to 'html'.
        /// </summary>
        [Arg("--format", order: int.MaxValue)]
        public string ReportFormat { get; set; }

        /// <summary>
        ///  A directory or file that needs to be excluded (can be repeated for multiple exclusions).
        /// </summary>
        [Arg("--exclude", order: int.MaxValue)]
        public List<string> Exclude { get; set; }

        /// <summary>
        /// Tested project path
        /// </summary>
        [Arg("--src", order: int.MaxValue)]
        public string TestPath { get; set; }
    }
}