namespace Metrics.Integrations.Linters.Phpmetrics
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
            return new LinterFileModel
            {
                Files = ((LintResult)result).FilesList.Cast<LinterFileModel.File>().ToList()
            };
        }
    }
}