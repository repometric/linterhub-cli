namespace Metrics.Integrations.Linters.htmlhint
{
    using System.IO;
    using Extensions;
    using System.Collections.Generic;

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
            var res = (LintResult)result;
            LinterFileModel lfm = new LinterFileModel();
            res.FilesList.ForEach(f =>
            {
                LinterFileModel.File lf = new LinterFileModel.File
                {
                    Path = f.FilePath
                };
                f.Messages.ForEach(e => 
                    lf.Errors.Add(new LinterError
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
                        }
                    })
                );
                lfm.Files.Add(lf);
            });
            return lfm;
        }


        /// <summary>
        /// For documentation look at Error class
        /// </summary>
        public class LinterError : LinterFileModel.Error
        {
            public string Type;
            public string Evidence;
            public string Raw;
        }
    }
}