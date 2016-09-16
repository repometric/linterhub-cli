namespace Metrics.Integrations.Linters.phpcheckstyle
{
    using Extensions;
    using System.IO;

    public class Lint : Linter
    {
        public override ILinterResult Parse(Stream stream)
        {
            return stream.DeserializeAsXml<LintResult>();
        }

        public override ILinterModel Map(ILinterResult result)
        {
            var res = (LintResult)result;
            LinterFileModel lfm = new LinterFileModel();
            foreach(File f in res.FilesList)
            {
                LinterFileModel.File lf = new LinterFileModel.File
                {
                    Path = f.FilePath
                };
                foreach (Error e in f.ErrorsList)
                    lf.Errors.Add(new LinterError {
                        Severity = e.Severity,
                        Message = e.Message,
                        Rule = new LinterFileModel.Rule
                        {
                            Name = e.Source
                        },
                        Row = new LinterFileModel.Interval
                        {
                            Start = System.Int32.Parse(e.Line),
                            End = System.Int32.Parse(e.Line)
                        },
                        Column = new LinterFileModel.Interval
                        {
                            Start = System.Int32.Parse(e.Column),
                            End = System.Int32.Parse(e.Column)
                        }
                    });
                // TODO: remove similar elements
            }
            return lfm;
        }

        public class LinterError : LinterFileModel.Error
        {
            public string Severity;
        }

    }
}