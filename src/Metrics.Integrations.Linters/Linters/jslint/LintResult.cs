namespace Metrics.Integrations.Linters.jslint
{
    using System.Collections.Generic;

    public class LintResult : ILinterResult
    {
        public List<File> FilesList {get; set;} 
    }
}