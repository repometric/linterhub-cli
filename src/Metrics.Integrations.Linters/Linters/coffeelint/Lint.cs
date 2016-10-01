namespace Metrics.Integrations.Linters.coffeelint
{
    using System.IO;
    using CsvHelper;
    using System.Linq;
    using System;

    public class Lint : Linter
    {
        public override ILinterResult Parse(Stream stream)
        {
            using (var csv = new CsvReader(new StreamReader(stream)))
            {
                csv.Configuration.RegisterClassMap<WarningMap>();
                return new LintResult
                {
                    Records = csv.GetRecords<Warning>().ToList()
                };
            }
        }

        public override ILinterModel Map(ILinterResult result)
        {
            return new LinterFileModel
            {
                Files = (from warning in ((LintResult)result).Records
                         group warning by warning.Path into g
                         select new LinterFileModel.File
                         {
                             Path = g.FirstOrDefault().Path,
                             Errors = g.Select(x => new LinterFileModel.Error
                             {
                                 Row = new LinterFileModel.Interval
                                 {
                                     Start = Int32.Parse(x.StartLine),
                                     End = Math.Max(Int32.Parse(x.StartLine), x.EndLine != string.Empty ? Int32.Parse(x.EndLine) : 0)
                                 },
                                 Message = x.Message,
                                 Severity = x.Severity,
                                 Line = Int32.Parse(x.StartLine)
                             }).ToList()
                         }).ToList()
            };
        }
    }
}