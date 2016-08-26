namespace Metrics.Integrations.Linters
{
    using System.Collections.Generic;

    public class LinterFileModel : ILinterModel
    {
        public List<File> Files { get; set; }

        public LinterFileModel()
        {
            Files = new List<File>();
        }

        public class File
        {
            public string Path { get; set; }
            public List<Error> Errors { get; set; }

            public File()
            {
                Errors = new List<Error>();
            }
        }

        public class Error
        {
            public string Message { get; set; }
            public Rule Rule { get; set; }
            public int Line { get; set; }
            public Interval Collum { get; set; }
            public Interval Row { get; set; }
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
    }
}
