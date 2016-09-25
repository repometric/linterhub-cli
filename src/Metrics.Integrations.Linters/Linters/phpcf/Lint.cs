namespace Metrics.Integrations.Linters.Phpcf
{
    using System.Text.RegularExpressions;
    using System.IO;
    using System.Linq;

    public class Lint : Linter
    {
        public override ILinterResult Parse(Stream stream)
        {
            var output = (new StreamReader(stream)).ReadToEnd();
            MatchCollection matches = Regex.Matches(output, @"\[([0-9]\.[0-9])\] (.*) in file (.*)\[([0-9]*)\]\. (.*)\r");
            return new LintResult
            {
                WarningsList = matches.Cast<Match>().Select(match => new Warning
                {
                    PhpVersion = match.Groups[1].Value,
                    Description = match.Groups[2].Value + ".",
                    FilePath = match.Groups[3].Value,
                    Line = match.Groups[4].Value,
                    Advice = match.Groups[5].Value
                }).ToList()
            };
        }

        public override ILinterModel Map(ILinterResult result)
        {
            return new LinterFileModel
            {
                Files = (from Warning in ((LintResult)result).WarningsList
                         group Warning by Warning.FilePath into g
                         select new LinterFileModel.File
                         {
                             Path = g.FirstOrDefault().FilePath,
                             Errors = g.Select(e => new LinterError
                             {
                                 Message = e.Description,
                                 Line = int.Parse(e.Line),
                                 PhpVersion = e.PhpVersion,
                                 Advice = e.Advice
                             }).Cast<LinterFileModel.Error>().ToList()
                         }).ToList()
            };
        }
    }
}