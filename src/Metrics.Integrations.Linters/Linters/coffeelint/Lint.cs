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

        public static LinterFileModel.Error.SeverityType SeverityConvertion(String s)
        {
            switch (s)
            {
                case "warn": return LinterFileModel.Error.SeverityType.warning;
                case "error": return LinterFileModel.Error.SeverityType.error;
                default: return LinterFileModel.Error.SeverityType.warning;
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
                                     Start = Int32.Parse(x.StartLine) - 1,
                                     End = Math.Max(Int32.Parse(x.StartLine), x.EndLine != string.Empty ? Int32.Parse(x.EndLine) : 0) - 1
                                 },
                                 Message = x.Message,
                                 Severity = SeverityConvertion(x.Severity),
                                 Line = Int32.Parse(x.StartLine) - 1,
                                 Column = new LinterFileModel.Interval
                                 {
                                     Start = 0, 
                                     End = 1000
                                 }
                             }).ToList()
                         }).ToList()
            };
        }
    }
}