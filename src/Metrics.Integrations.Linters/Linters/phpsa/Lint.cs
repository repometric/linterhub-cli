namespace Metrics.Integrations.Linters.Phpsa
{
    using System.IO;
    using Extensions;
    using System.Collections.Generic;
    using System.Linq;

    public class Lint : Linter
    {
        public override ILinterResult Parse(Stream stream)
        {

            return new LintResult
            {
                ErrorsList = stream.DeserializeAsJson<List<Error>>()
            };
        }

        public override ILinterModel Map(ILinterResult result)
        {
            var res = (LintResult)result;
            LinterFileModel lfm = new LinterFileModel();
               
            foreach(Error e in res.ErrorsList)
            {
                LinterFileModel.File lf;
                if (!lfm.Files.Exists(x => x.Path == e.File))
                    lf= new LinterFileModel.File
                    {
                        Path = e.File
                    };
                else
                {
                    lf = lfm.Files.Find(x => x.Path == e.File);
                    lfm.Files.Remove(lf);
                }

                lf.Errors.Add(new LinterError
                {
                    Message = e.Message,
                    Line = e.Line,
                    Type = e.Type
                });
                lfm.Files.Add(lf);
            }

            return lfm;
        }

        public class LinterError : LinterFileModel.Error
        {
            public string Type;
        }

    }
}