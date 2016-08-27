namespace Metrics.Integrations.Linters.Phpmd
{
    public class LintArgs : ILinterArgs
    {
        /// <summary>
        /// Report type. Default: XML
        /// </summary>
        [Arg("", separator: "", order: 2)]
        public string ReportType { get; set; } 

        /// <summary>
        /// Tool path
        /// </summary>
        [Arg("", order: int.MinValue)]
        public string ToolPath { get; set; }

        /// <summary>
        /// Ruleset. For more information visit: https://phpmd.org/rules/index.html
        /// </summary>
        [Arg("", order: int.MaxValue)]
        public string Rulesets { get; set; }

        /// <summary>
        /// Tested project path
        /// </summary>
        [Arg("", order: 1)]
        public string TestPath { get; set; }
    }
}