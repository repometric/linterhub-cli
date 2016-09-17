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
            return new LintResult
            {
                Records = new CsvReader(new StreamReader(stream)).GetRecords<Warning>().ToList()
            };
        }

        public override ILinterModel Map(ILinterResult result)
        {
            LinterFileModel lfm = new LinterFileModel();
            var res = (LintResult)result;
            res.Records.ForEach(x => {
                if (!lfm.Files.Exists(f => f.Path == x.path))
                    lfm.Files.Add(new LinterFileModel.File
                    {
                        Path = x.path
                    });

                lfm.Files.Find(f => f.Path == x.path).Errors.Add(new LinterFileModel.Error
                {
                    Row = new LinterFileModel.Interval
                    {
                        Start = Int32.Parse(x.lineNumber),
                        End = Math.Max(Int32.Parse(x.lineNumber), x.lineNumberEnd != "" ? Int32.Parse(x.lineNumberEnd) : 0)
                    },
                    Message = x.message,
                    Severity = x.level,
                    Line = Int32.Parse(x.lineNumber)
                });

            });
            return lfm;
        }

    }
}