namespace Metrics.Integrations.Linters.htmlhint
{
    using System.IO;
    using Extensions;
    using System.Collections.Generic;
    using System.Linq;

    public class Lint : Linter
    {
        public override ILinterResult Parse(Stream stream)
        {
            return new LintResult
            {
                FilesList = stream.DeserializeAsJson<List<File>>()
            };
        }

        public override ILinterModel Map(ILinterResult result)
        {
            return new LinterFileModel
            {
                Files = ((LintResult)result).FilesList.Select(f => new LinterFileModel.File
                {
                    Path = f.FilePath,
                    Errors = f.Messages.Select(e => new LinterError
                    {
                        Column = new LinterFileModel.Interval
                        {
                            Start = e.Column,
                            End = e.Column
                        },
                        Line = e.Line,
                        Message = e.Message,
                        Evidence = e.Evidence,
                        Type = e.Type,
                        Raw = e.Raw,
                        Rule = new LinterFileModel.Rule
                        {
                            Name = e.Rule.Description,
                            Id = e.Rule.Id
                        },
                        Severity = e.Severity
                    }).Cast<LinterFileModel.Error>().ToList()
                }).ToList()
            };
        }
    }
}