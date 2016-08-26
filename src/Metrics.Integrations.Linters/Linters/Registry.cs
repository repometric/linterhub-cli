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
                    Linter = typeof(Metrics.Integrations.Linters.Phpcs.Lint),
                    Args = typeof(Metrics.Integrations.Linters.Phpcs.LintArgs),
                    Result = typeof(Metrics.Integrations.Linters.Phpcs.LintResult),
                    Model = typeof(Metrics.Integrations.Linters.Phpcs.LintResult)
                }
            };
        }
    }
}
