namespace Metrics.Integrations.Linters.Phpsa
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
                ErrorsList = stream.DeserializeAsJson<List<Error>>()
            };
        }

        public override ILinterModel Map(ILinterResult result)
        {
            return (ILinterModel)result;
        }

    }
}