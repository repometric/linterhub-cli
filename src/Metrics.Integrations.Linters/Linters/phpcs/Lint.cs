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

        public override ILinterModel Map(ILinterResult result) {
            var res = (LintResult)result;
            LinterFileModel lfm = new LinterFileModel();
            foreach (KeyValuePair<string, Phpcs.File> kvp in res.Files)
            {
                LinterFileModel.File lf = new LinterFileModel.File();
                lf.Path = kvp.Key;
                foreach (var error in kvp.Value.Messages)
                {
                    LinterFileModel.Error le = new LinterFileModel.Error();
                    le.Message = error.Message;
                    var lr = new LinterFileModel.Rule();
                    lr.Name = error.Source;
                    le.Rule = lr;
                    le.Line = error.Line;
                    var li = new LinterFileModel.Interval();
                    li.Start = error.Column;
                    li.End = error.Column;
                    le.Column = li;
                    if(error.Type == "ERROR")
                        le.Type = LinterFileModel.ErrorType.Error;
                    else le.Type = LinterFileModel.ErrorType.Warning;
                    lf.Errors.Add(le);
                }
                lfm.Files.Add(lf);
            }
            return lfm;
        }
    }
}