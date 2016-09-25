namespace Metrics.Integrations.Linters.csslint
{
    using System.Collections.Generic;

    public class LintResult : ILinterResult
    {
        /// <summary>
        ///  List of tested files
        /// </summary>
        public List<File> FilesList { get; set; }
    }
}