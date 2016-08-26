namespace Metrics.Integrations.Linters
{
    using System;
    using System.Collections.Generic;

    internal static class Registry
    {
        internal class Record
        {
            public string Name { get; set; }
            public Type Linter { get; set; }
            public Type Args { get; set; }
            public Type Result { get; set; }
            public Type Model { get; set; }
        }

        internal static IEnumerable<Record> Get()
        {
            return new[]
            {
                new Record {
                    Name = "phpcs",
                    Linter = typeof(Phpcs.Lint),
                    Args = typeof(Phpcs.LintArgs),
                    Result = typeof(Phpcs.LintResult),
                    Model = typeof(LinterFileModel)
                },
                new Record {
                    Name = "phpmd",
                    Linter = typeof(Phpmd.Lint),
                    Args = typeof(Phpmd.LintArgs),
                    Result = typeof(Phpmd.LintResult),
                    Model = typeof(Phpmd.LintResult)
                }
            };
        }
    }
}
