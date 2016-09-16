namespace Metrics.Integrations.Linters.phpcheckstyle
{
    using Extensions;
    using System.IO;
    using System;

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
                {
                    LinterError le = new LinterError
                    {
                        Severity = e.Severity,
                        Message = e.Message,
                        Rule = new LinterFileModel.Rule
                        {
                            Name = e.Source
                        },
                        Row = new LinterFileModel.Interval
                        {
                            Start = Int32.Parse(e.Line),
                            End = Int32.Parse(e.Line)
                        },
                        Column = new LinterFileModel.Interval
                        {
                            Start = Int32.Parse(e.Column),
                            End = Int32.Parse(e.Column)
                        }
                    };
                    LinterError le_ = (LinterError)lf.Errors.Find(x => 
                        x.Message == le.Message && 
                        x.Rule == le.Rule &&
                        (x.Row.Start == le.Row.Start + 1) || (x.Row.End == le.Row.End - 1) 
                    );
                    if (le_ != null)
                    {
                        lf.Errors.Remove(le_);
                        le_.Row.Start = Math.Min(le.Row.Start, le_.Row.Start);
                        le_.Row.End = Math.Max(le.Row.End, le_.Row.End);
                        lf.Errors.Add(le_);
                    }
                    else lf.Errors.Add(le);
                }
                lfm.Files.Add(lf);
            }
            return lfm;
        }

        public class LinterError : LinterFileModel.Error
        {
            public string Severity;
        }

    }
}