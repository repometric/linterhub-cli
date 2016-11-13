namespace Linterhub.Engine.Linters.Phpmetrics
{
    using System.Collections.Generic;

    public class LintResult : ILinterResult
    {
        public List<File> FilesList { get; set; }
    }
}