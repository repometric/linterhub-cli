namespace Metrics.Integrations.Linters.Phpcs
{
    using System.IO;
    using Extensions;
    using System.Collections.Generic;
    using System.Linq;

    public class Lint : Linter
    {
        public override ILinterResult Parse(Stream stream)
        {
            return stream.DeserializeAsJson<LintResult>();
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
                        Severity = error.lfmSeverity
                    }).ToList()
                }).ToList()
            };
        }
    }
}