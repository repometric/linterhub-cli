namespace Linterhub.Engine.Schema
{
    using System.Collections.Generic;

    public class LinterhubOutputSchema : List<LinterhubOutputSchema.LinterOutput>
    {
        public class LinterOutput
        {
            public LinterOutputSchema Files { get; set; }
            public string Name { get; set; }
            public Error Error { get; set; }
        }

        public class Error
        {
            public string Message { get; set; }
            public string Output { get; set; }
        }
    }
}
