namespace Linterhub.Engine.Linters.phpcheckstyle
{
    using Extensions;
    using System;
    using System.IO;
    using System.Linq;

    public class Lint : Linter
    {
        public override ILinterResult Parse(Stream stream)
        {
            return stream.DeserializeAsXml<LintResult>();
        }

        public static LinterFileModel.Error.SeverityType SeverityConvertion(String s)
        {
            switch (s)
            {
                case "warning": return LinterFileModel.Error.SeverityType.warning;
                case "error": return LinterFileModel.Error.SeverityType.error;
                default: return LinterFileModel.Error.SeverityType.warning;
            }
        }

        public override ILinterModel Map(ILinterResult result)
        {
            return new LinterFileModel
            {
                Files = ((LintResult)result).FilesList.Select(z => new LinterFileModel.File
                {
                    Path = z.FilePath,
                    Errors = z.ErrorsList.Select(e => new LinterFileModel.Error
                    {
                        Severity = SeverityConvertion(e.Severity),
                        Message = e.Message,
                        Rule = new LinterFileModel.Rule
                        {
                            Name = e.Source
                        },
                        Row = new LinterFileModel.Interval
                        {
                            Start = int.Parse(e.Line),
                            End = int.Parse(e.Line)
                        },
                        Column = new LinterFileModel.Interval
                        {
                            Start = int.Parse(e.Column),
                            End = int.Parse(e.Column)
                        }
                    }).ToList()
                }).ToList()
            };
        }
    }
}