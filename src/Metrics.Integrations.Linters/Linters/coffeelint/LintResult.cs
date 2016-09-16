namespace Metrics.Integrations.Linters.coffeelint
{
    using System.Collections.Generic;

    public class LintResult : ILinterResult, ILinterModel
    {
        public List<Warning> Records = new List<Warning>();
    }
}
