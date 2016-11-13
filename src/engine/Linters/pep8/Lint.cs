namespace Linterhub.Engine.Linters.pep8
{
    using System.Text.RegularExpressions;
    using System.IO;
    using System.Linq;

    public class Lint : Linter
    {
        public override ILinterResult Parse(Stream stream)
        {
            using (StreamReader sr = new StreamReader(stream))
            {
                var output = sr.ReadToEnd();
                MatchCollection matches = Regex.Matches(output, @".\/(.*)\:([0-9]*): \[(.*)\] (.*)");
                return new LintResult
                {
                    WarningsList = matches.Cast<Match>().Select(match => new Warning
                    {
                        Description = match.Groups[4].Value + ".",
                        FilePath = match.Groups[1].Value,
                        Line = match.Groups[2].Value,
                        Pattern = match.Groups[3].Value
                    }).ToList()
                };
            }
        }

        public override ILinterModel Map(ILinterResult result)
        {
            return new LinterFileModel
            {
                Files = (from warning in ((LintResult)result).WarningsList
                         group warning by warning.FilePath into g
                         select new LinterFileModel.File
                         {
                             Path = g.FirstOrDefault().FilePath,
                             Errors = g.Select(e => new LinterFileModel.Error
                             {
                                 Message = e.Description,
                                 Line = int.Parse(e.Line),
                                 Row = new LinterFileModel.Interval
                                 {
                                     Start = int.Parse(e.Line),
                                     End = int.Parse(e.Line)
                                 },
                                 Rule = new LinterFileModel.Rule
                                 {
                                     Name = e.Pattern
                                 }
                             }).Cast<LinterFileModel.Error>().ToList()
                         }).ToList()
            };
        }
    }
}