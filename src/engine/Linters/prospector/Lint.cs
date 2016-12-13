namespace Linterhub.Engine.Linters.prospector
{
    using Extensions;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class Lint : Linter
    {
        public override ILinterResult Parse(Stream stream)
        {
            return stream.DeserializeAsJson<LintResult>();
        }

        public override ILinterModel Map(ILinterResult result)
        {
            return new LinterFileModel
            {
                Files = (from error in ((LintResult)result).MessagesList
                         group error by error.WarningLocation.Path into g
                         select new LinterFileModel.File
                         {
                             Path = g.First().WarningLocation.Path,
                             Errors = g.Select(e => new LinterFileModel.Error
                             {
                                 Message = e.Description,
                                 Line = e.WarningLocation.Line ?? 0 ,
                                 Row = new LinterFileModel.Interval
                                 {
                                     Start = e.WarningLocation.Line ?? 0,
                                     End = e.WarningLocation.Line ?? 0
                                 },
                                 Column = new LinterFileModel.Interval
                                 {
                                     Start = e.WarningLocation.Column ?? 0,
                                     End = e.WarningLocation.Column ?? 0 
                                 },
                                 Rule = new LinterFileModel.Rule
                                 {
                                     Namespace = e.Source,
                                     Id = e.WarningCode,
                                     Name = e.WarningCode
                                 }
                             }).ToList()
                         }).ToList()
            };
        }
    }
}