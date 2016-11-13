namespace Linterhub.Engine.Linters.pep8
{
    using System.Collections.Generic;

    public class LintResult : ILinterResult
    {
        public List<Warning> WarningsList { get; set; }
    }
}
