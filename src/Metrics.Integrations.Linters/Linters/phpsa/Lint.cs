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
            return new LinterFileModel
            {
                Files = (from Error in ((LintResult)result).ErrorsList
                         group Error by Error.File into g
                         select new LinterFileModel.File
                         {
                             Path = g.FirstOrDefault().File,
                             Errors = g.Select(e => new LinterFileModel.Error
                             {
                                 Message = e.Message,
                                 Line = e.Line,
                                 Severity = e.Type
                             }).ToList()
                         }).ToList()
            };
        }
    }
}