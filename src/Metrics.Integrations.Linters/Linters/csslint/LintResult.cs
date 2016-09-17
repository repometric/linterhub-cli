namespace Metrics.Integrations.Linters.csslint
{
    using System.Collections.Generic;

    public class LintResult : ILinterResult, ILinterModel
    {
        public List<File> FilesList { get; set; }
    }
}