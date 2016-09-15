namespace Metrics.Integrations.Linters
{
    using System;
    using System.Collections.Generic;

    public static class Registry
    {
        public class Record
        {
            public string Name { get; set; }
            public Type Linter { get; set; }
            public Type Args { get; set; }
            public Type Result { get; set; }
            public Type Model { get; set; }
        }

        public static IEnumerable<Record> Get()
        {
            return new[]
            {
                new Record
                {
                    Name = "phpcs",
                    Linter = typeof(Phpcs.Lint),
                    Args = typeof(Phpcs.LintArgs),
                    Result = typeof(Phpcs.LintResult),
                    Model = typeof(LinterFileModel)
                },
                new Record
                {
                    Name = "phpmd",
                    Linter = typeof(Phpmd.Lint),
                    Args = typeof(Phpmd.LintArgs),
                    Result = typeof(Phpmd.LintResult),
                    Model = typeof(LinterFileModel)
                },
                new Record
                {
                    Name = "phpmetrics",
                    Linter = typeof(Phpmetrics.Lint),
                    Args = typeof(Phpmetrics.LintArgs),
                    Result = typeof(Phpmetrics.LintResult),
                    Model = typeof(Phpmetrics.LintResult)
                },
                new Record
                {
                    Name = "phpsa",
                    Linter = typeof(Phpsa.Lint),
                    Args = typeof(Phpsa.LintArgs),
                    Result = typeof(Phpsa.LintResult),
                    Model = typeof(Phpsa.LintResult)
                },
                new Record
                {
                    Name = "phpcpd",
                    Linter = typeof(Phpcpd.Lint),
                    Args = typeof(Phpcpd.LintArgs),
                    Result = typeof(Phpcpd.LintResult),
                    Model = typeof(Phpcpd.LintResult)
                },
                new Record
                {
                    Name = "phpassumptions",
                    Linter = typeof(PhpAssumptions.Lint),
                    Args = typeof(PhpAssumptions.LintArgs),
                    Result = typeof(PhpAssumptions.LintResult),
                    Model = typeof(PhpAssumptions.LintResult)
                }
            };
        }
    }
}
