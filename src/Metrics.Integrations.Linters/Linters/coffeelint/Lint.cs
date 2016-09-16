namespace Metrics.Integrations.Linters.coffeelint
{
    using System.IO;
    using CsvHelper;
    using System.Linq;

    public class Lint : Linter
    {
        public override ILinterResult Parse(Stream stream)
        {
            return new LintResult
            {
                Records = new CsvReader(new StreamReader(stream)).GetRecords<Warning>().ToList()
            };
        }

        public override ILinterModel Map(ILinterResult result)
        {
            return (ILinterModel)result;
        }

    }
}