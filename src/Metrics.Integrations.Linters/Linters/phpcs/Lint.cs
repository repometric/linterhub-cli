namespace Metrics.Integrations.Linters.Phpcs
{
    using System.IO;
    using Extensions;
    using System.Collections.Generic;

    public class Lint : Linter
    {
        public override ILinterResult Parse(Stream stream)
        {
            return stream.DeserializeAsJson<LintResult>();
        }

        public override ILinterModel Map(ILinterResult result)
        {
            var res = (LintResult)result;
            LinterFileModel lfm = new LinterFileModel();
            foreach (KeyValuePair<string, Phpcs.File> kvp in res.Files)
            {
                LinterFileModel.File lf = new LinterFileModel.File
                {
                    Path = kvp.Key
                };
                foreach (var error in kvp.Value.Messages)
                {
                    LinterError le = new LinterError
                    {
                        Message = error.Message,
                        Rule = new LinterFileModel.Rule
                        {
                            Name = error.Source
                        },
                        Line = error.Line,
                        Column = new LinterFileModel.Interval{
                            Start = error.Column,
                            End = error.Column
                        },
                        Type = error.Type == LinterError.ERROR ? LinterError.ErrorType.Error : LinterError.ErrorType.Warning
                    };
                    lf.Errors.Add(le);
                }
                lfm.Files.Add(lf);
            }
            return lfm;
        }

        public class LinterError : LinterFileModel.Error
        {
            public ErrorType Type;
            public const string ERROR = "ERROR";
            public enum ErrorType { Warning, Error };
        }
    }
}