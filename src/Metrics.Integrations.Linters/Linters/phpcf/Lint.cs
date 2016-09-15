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
            return (ILinterModel)result;
        }

    }
}