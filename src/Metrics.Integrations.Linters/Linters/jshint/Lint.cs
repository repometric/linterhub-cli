namespace Metrics.Integrations.Linters.jshint
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
                    LinterFileModel.Error le = new LinterFileModel.Error
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
                    lf.Errors.Add(le);
                }
                lfm.Files.Add(lf);
            }
            return lfm;
        }

    }
}