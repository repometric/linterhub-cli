namespace Metrics.Integrations.Linters.pmd
{
    using System.IO;
    using Extensions;
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
                Files = ((LintResult)result).Files.Select(file => new LinterFileModel.File
                {
                    Path = file.FileName,
                    Errors = file.ViolationsList.Select(error => new LinterError
                    {
                        Variable = error.Variable,
                        ExternalInfoUrl = error.ExternalInfoUrl,
                        Priority = error.Priority,

                        Row = new LinterFileModel.Interval
                        {
                            Start = error.BeginLine,
                            End = error.EndLine
                        },
                        Column = new LinterFileModel.Interval
                        {
                            Start = error.BeginColumn,
                            End = error.Endcolumn
                        },
                        
                        Message = error.Description.Trim(),
                        Rule = new LinterFileModel.Rule()
                        {
                            Name = error.Rule,
                            Namespace = error.RuleSet
                        },
                        ErrorLocation = new LinterError.Location
                        {
                            Class = error.Class,
                            Method = error.Method,
                            Package = error.Package
                        }
                    }).Cast<LinterFileModel.Error>().ToList()
                }).ToList()
            };
        }
    }
}