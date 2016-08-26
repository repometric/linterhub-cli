namespace Metrics.Integrations.Linters.Phpcs
{
    using System.IO;
    using Extensions;

    public class Lint : Linter
    {
        public override ILinterResult Parse(Stream stream)
        {
            return stream.DeserializeAsJson<LintResult>();
        }

        public override ILinterModel Map(ILinterResult result)
        {
            return (LintResult)result;
        }
    }
}