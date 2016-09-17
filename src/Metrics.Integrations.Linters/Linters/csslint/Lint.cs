namespace Metrics.Integrations.Linters.csslint
{
    using Extensions;
    using System.Collections.Generic;
    using System.IO;
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
            LinterFileModel lfm = new LinterFileModel();
            var res = (LintResult)result;
            res.FilesList.ForEach(x => {
                lfm.Files.Add(new LinterFileModel.File
                {
                    Path = x.FilePath,
                    Errors = x.MessagesList.Select(z => new LinterFileModel.Error
                    {
                        Message = z.ErrorMessage,
                        Column = new LinterFileModel.Interval
                        {
                            Start = z.Column,
                            End = z.Column
                        },
                        Severity = z.Severity,
                        Row = new LinterFileModel.Interval
                        {
                            Start = z.Line,
                            End = z.Line
                        },
                        Rule = new LinterRule
                        {
                            Description = z.LRule.Description, 
                            Id = z.LRule.Id,
                            Name = z.LRule.Name,
                            Browser = z.LRule.Browsers,
                            GithubLink = z.LRule.GithubUrl
                        },
                        Evidence = z.Evidence
                    }).ToList()
                });
            });
            return lfm;
        }

        public class LinterRule : LinterFileModel.Rule
        {
            public string Description;
            public string Browser;
            public string GithubLink;
        }

    }
}