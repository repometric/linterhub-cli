namespace Metrics.Intergations.Linters
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
                new Record { Name = "default", Linter = typeof(ILinter), Args = typeof(ILinterArgs), Result = typeof(ILinterResult), Model = typeof(ILinterModel) }
            };
        }
    }
}
