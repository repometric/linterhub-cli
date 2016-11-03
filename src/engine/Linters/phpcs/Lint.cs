namespace Linterhub.Engine.Linters.Phpcs
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
            return stream.DeserializeAsJson<LintResult>();
        }

        public static LinterFileModel.Error.SeverityType SeverityConvertion(String s)
        {
            switch (s)
            {
                case "WARNING": return LinterFileModel.Error.SeverityType.warning;
                case "ERROR": return LinterFileModel.Error.SeverityType.error;
                default: return LinterFileModel.Error.SeverityType.warning;
            }
        }

        public override ILinterModel Map(ILinterResult result)
        {
            return new LinterFileModel
            {
                Files = ((LintResult)result).Files.Select(kvp => new LinterFileModel.File
                {
                    Path = kvp.Key,
                    Errors = kvp.Value.Messages.Select(error => new LinterFileModel.Error
                    {
                        Message = error.Message,
                        Rule = new LinterFileModel.Rule
                        {
                            Name = error.Source
                        },
                        Line = error.Line,
                        Row = new LinterFileModel.Interval
                        {
                            Start = error.Line,
                            End = error.Line
                        },
                        Column = new LinterFileModel.Interval
                        {
                            Start = 0,
                            End = 1000
                        },
                        Severity = SeverityConvertion(error.Type)
                    }).ToList()
                }).ToList()
            };
        }
    }
}