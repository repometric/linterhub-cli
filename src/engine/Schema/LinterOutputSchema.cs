namespace Linterhub.Engine.Schema
{
    using System.Collections.Generic;

    public class LinterOutputSchema : List<LinterOutputSchema.File>
    {
        public class File
        {
            public string FilePath { get; set; }
            public IEnumerable<FileMessage> Messages { get; set; }

            public File()
            {
                Messages = new List<FileMessage>();
            }
        }

        public class FileMessage
        {
            public string Message { get; set; }
            public string Description { get; set; }
            public string Context { get; set; }
            public string Source { get; set; }
            public SeverityType Severity { get; set; }
            public string Column { get; set; }
            public string ColumnEnd { get; set; }
            public string Line { get; set; }
            public string LineEnd { get; set; }
            public string RuleId { get; set; }
        }

        public class Rule
        {
            public string Name { get; set; }
            public string Id { get; set; }
            public string Namespace { get; set; }
        }

        public class Interval
        {
            public int Start { get; set; }
            public int End { get; set; }
        }

        public enum SeverityType
        {
            Unknown = 0,
            Verbose,
            Hint,
            Information,
            Warning,
            Error,
        }
    }
}
