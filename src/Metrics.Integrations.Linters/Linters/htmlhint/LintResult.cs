namespace Metrics.Integrations.Linters.htmlhint
{
    using System.Collections.Generic;

    public class LintResult : ILinterResult, ILinterModel
    {
        public List<File> FilesList {get; set;} 
    }
}