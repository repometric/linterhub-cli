namespace Linterhub.Engine.Linters.htmlhint
{
    using System.IO;
    using Extensions;
    using System.Collections.Generic;
    using System.Linq;
    using System;

    public class Lint : Linter
    {
        public override ILinterResult Parse(Stream stream)
        {
            return new LintResult
            {
                FilesList = stream.DeserializeAsJson<List<File>>()
            };
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
                        Type = e.Severity,
                        Raw = e.Raw,
                        Rule = new LinterFileModel.Rule
                        {
                            Name = e.Rule.Description,
                            Id = e.Rule.Id
                        },
                        Severity = SeverityConvertion(e.Severity)
                    }).Cast<LinterFileModel.Error>().ToList()
                }).ToList()
            };
        }
    }
}