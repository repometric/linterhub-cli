namespace Linterhub.Engine.Linters.coffeelint
{
    public class LintArgs : ILinterArgs
    {

        public LintArgs()
        {
            CoffeeLint = true;
            ReportType = "csv";
        }

        /// <summary>
        /// Tested project path
        /// </summary>
        [ArgPath]
        public string Path { get; set; }

        /// <summary>
        /// Tool path
        /// </summary>
        [Arg("coffeelint", false, order: int.MinValue)]
        public bool CoffeeLint { get; set; }

        /// <summary>
        ///  Built in reporter (default, csv, jslint, checkstyle, raw),
        ///  or module, or path to reporter file.
        /// </summary>
        [Arg("--reporter", order: 0)]
        public string ReportType { get; set; }

        /// <summary>
        ///  Specify a custom configuration file.
        /// </summary>
        [Arg("--file", order: 0)]
        public string ConfigFile { get; set; }

        /// <summary>
        ///  Specify a custom rule or directory of rules.
        /// </summary>
        [Arg("--rules", order: 0)]
        public string CustomRules { get; set; }

        /// <summary>
        ///  Specify an additional file extension, separated by comma.
        /// </summary>
        [Arg("--ext", order: 0)]
        public string Extensions { get; set; }
    }
}