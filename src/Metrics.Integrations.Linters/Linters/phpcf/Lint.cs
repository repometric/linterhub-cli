namespace Metrics.Integrations.Linters.Phpcf
{
    using System.Text.RegularExpressions;
    using System.IO;

    public class Lint : Linter
    {
        public override ILinterResult Parse(Stream stream)
        {
            var output = (new StreamReader(stream)).ReadToEnd();
            MatchCollection matches = Regex.Matches(output, @"\[([0-9]\.[0-9])\] (.*) in file (.*)\[([0-9]*)\]\. (.*)\r");
            LintResult lr = new LintResult();
            foreach (Match match in matches)
            {
                try
                {
                    lr.WarningsList.Add(new Warning{
                        PhpVersion = match.Groups[1].Value,
                        Description = match.Groups[2].Value + ".",
                        FilePath = match.Groups[3].Value,
                        Line = match.Groups[4].Value,
                        Advice = match.Groups[5].Value
                    });
                }
                catch { }
            }
            return lr;
        }

        public override ILinterModel Map(ILinterResult result)
        {
            var res = (LintResult)result;
            LinterFileModel lfm = new LinterFileModel();

            foreach (Warning e in res.WarningsList)
            {
                LinterFileModel.File lf;
                if (!lfm.Files.Exists(x => x.Path == e.FilePath))
                    lf = new LinterFileModel.File
                    {
                        Path = e.FilePath
                    };
                else
                {
                    lf = lfm.Files.Find(x => x.Path == e.FilePath);
                    lfm.Files.Remove(lf);
                }

                lf.Errors.Add(new LinterError
                {
                    Message = e.Description,
                    Line = System.Int32.Parse(e.Line),
                    PhpVersion = e.PhpVersion,
                    Advice = e.Advice
                });
                lfm.Files.Add(lf);
            }

            return lfm;
        }

        public class LinterError : LinterFileModel.Error
        {
            public string PhpVersion;

            /// <summary>
            ///  Advice how to improve code
            /// </summary>
            public string Advice;
        }

    }
}