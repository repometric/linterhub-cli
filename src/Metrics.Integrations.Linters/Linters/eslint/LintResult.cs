namespace Metrics.Integrations.Linters.eslint
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
