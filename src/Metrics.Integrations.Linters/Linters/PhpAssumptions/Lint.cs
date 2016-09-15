namespace Metrics.Integrations.Linters.PhpAssumptions
{
    using Extensions;
    using System.IO;

    public class Lint : Linter
    {
        public override ILinterResult Parse(Stream stream)
        {
            return stream.DeserializeAsXml<LintResult>();
        }

        public override ILinterModel Map(ILinterResult result)
        {
            return (ILinterModel)result;
        }

    }
}