namespace Linterhub.Engine.Linters.csslint
{
    using Extensions;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class Lint : Linter
    {
        public override ILinterResult Parse(Stream stream)
        {
            try
            {
                return new LintResult
                {
                    FilesList = stream.DeserializeAsJson<List<File>>()
                };
            }
            catch (Exception)
            {
                stream.Position = 0;
                return new LintResult
                {
                    FilesList = new List<File>
                    {
                        stream.DeserializeAsJson<File>()
                    }
                };
            }
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
                Files = ((LintResult)result).FilesList.Select(x => new LinterFileModel.File
                {
                    Path = x.FilePath.Substring(8),
                    Errors = x.MessagesList.Select(z => new LinterFileModel.Error
                    {
                        Message = z.ErrorMessage,
                        Column = new LinterFileModel.Interval
                        {
                            Start = z.Column,
                            End = z.Column
                        },
                        Severity = SeverityConvertion(z.Severity),
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
                }).ToList()
            };
        }
    }
}