namespace Metrics.Integrations.Linters.Phpsa
{
    using System.Collections.Generic;

    public class LintResult : ILinterResult, ILinterModel
    {
        /// <summary>
        /// List of errors
        /// </summary>
        public List<Error> ErrorsList { get; set; }
    }
}