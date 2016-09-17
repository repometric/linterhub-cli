namespace Metrics.Integrations.Linters.PhpAssumptions
{
    using Extensions;
    using System.IO;
    using System.Linq;

    public class Lint : Linter
    {
        public override ILinterResult Parse(Stream stream)
        {
            return stream.DeserializeAsXml<LintResult>();
        }

        public override ILinterModel Map(ILinterResult result)
        {
            LinterFileModel lfm = new LinterFileModel();
            var res = (LintResult)result;
            res.FilesList.ForEach(x => {
                lfm.Files.Add(new LinterFileModel.File
                {
                    Path = x.FilePath,
                    Errors = x.LinesList.Select(z => new LinterFileModel.Error()
                    {
                        Message = z.Message,
                        Line = System.Int32.Parse(z.LineNumber)
                    }).ToList()
                });
            });
            return lfm;
        }

    }
}