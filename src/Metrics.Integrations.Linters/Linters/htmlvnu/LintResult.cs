namespace Metrics.Integrations.Linters.htmlvnu
{
    using System.Collections.Generic;

    public class LintResult : ILinterResult
    {
        public List<Message> Messages { get; set; }
    }
}
