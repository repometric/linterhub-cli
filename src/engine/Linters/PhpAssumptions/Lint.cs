namespace Linterhub.Engine.Linters.PhpAssumptions
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
            return new LinterFileModel
            {
                Files = ((LintResult)result).FilesList.Select<File, LinterFileModel.File>(x => new LinterFileModel.File
                {
                    Path = x.FilePath,
                    Errors = x.LinesList.Select(z => new LinterFileModel.Error()
                    {
                        Message = z.Message,
                        Line = System.Int32.Parse(z.LineNumber)
                    }).ToList()
                }).ToList()
            };
        }

    }
}