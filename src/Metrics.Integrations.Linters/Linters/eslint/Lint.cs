namespace Metrics.Integrations.Linters.eslint
{

    using System.IO;
    using System.Collections.Generic;
    using System.Linq;
    using Extensions;

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
            
            var model = new LinterFileModel
            {
                Files = ((LintResult)result).FilesList.Select(file => (LinterFileModel.File) new LinterFile
                {
                    Path = file.FilePath,
                    ErrorCount = file.ErrorCount,
                    WarningCount = file.WarningCount,
                    Errors = file.Messages.Select(fileError => (LinterFileModel.Error) new LinterError
                    {
                        Line = fileError.Line,
                        Message = fileError.MessageError,
                        Column = new LinterFileModel.Interval
                        {
                            Start = fileError.Column
                        }, 
                        Rule = new LinterFileModel.Rule
                        {
                            Id = fileError.RuleId
                        },
                        Severity = LinterFileModel.Error.SeverityType.error,
                        NodeType = fileError.NodeType,
                        Evidence = fileError.Source
                    }).ToList()
                }).ToList()
            };

            return model;
        }
    }
}