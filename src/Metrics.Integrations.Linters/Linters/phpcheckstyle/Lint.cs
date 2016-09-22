namespace Metrics.Integrations.Linters.phpcheckstyle
{
    using Extensions;
    using System.IO;
    using System.Linq;

    public class Lint : Linter
    {
        public override ILinterResult Parse(Stream stream)
        {
            return stream.DeserializeAsXml<LintResult>();
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
                        Severity = e.Severity,
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