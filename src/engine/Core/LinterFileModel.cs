namespace Linterhub.Engine
{
    using System.Collections.Generic;

    public class LinterFileModel : ILinterModel
    {
        public List<File> Files { get; set; }
        public string ErrorParse { get; set; }

        public LinterFileModel()
        {
            Files = new List<File>();
            ErrorParse = string.Empty;
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
            public enum SeverityType { error, warning, information, hint}
            public string Message { get; set; }
            public Rule Rule { get; set; }
            public SeverityType Severity { get; set; }
            public string Evidence { get; set; }
            public int Line { get; set; }
            public Interval Column { get; set; }
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
