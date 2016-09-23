namespace Metrics.Integrations.Linters.Phpmd
{
    using System.Linq;
    using Extensions;
    using System.IO;

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
                Files = ((LintResult)result).FilesList.Select(file => new LinterFileModel.File
                {
                    Path = file.FileName,
                    Errors = file.ViolationsList.Select(error => new LinterError
                    {
                        Row = new LinterFileModel.Interval
                        {
                            Start = error.BeginLine,
                            End = error.EndLine
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