namespace Metrics.Integrations.Linters.Phpmd
{
    using System;
    using Extensions;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;

    public class Lint : Linter
    {
        public override ILinterResult Parse(Stream stream)
        {
            return stream.DeserializeAsXml<LintResult>();
        }

        public override ILinterModel Map(ILinterResult result)
        {
            var res = (LintResult)result;
            LinterFileModel lfm = new LinterFileModel();
            foreach (var file in res.FilesList)
            {
                LinterFileModel.File lf = new LinterFileModel.File
                {
                    Path = file.FileName
                };
                //file.ViolataionsList.Select(x => new LinterError { ... })
                foreach (var error in file.ViolationsList)
                {
                    LinterError le = new LinterError
                    {
                        Row = new LinterFileModel.Interval 
                        { 
                            Start = error.BeginLine, 
                            End = error.EndLine 
                        },
                        Message = error.Description.Trim(),
                        Rule = new LinterFileModel.Rule(){
                            Name = error.Rule,
                            Namespace = error.RuleSet
                        },
                        ErrorLocation = new LinterError.Location{
                            Class = error.Class,
                            Method = error.Method,
                            Package = error.Package
                        }
                    };
                    lf.Errors.Add(le);
                }
                lfm.Files.Add(lf);
            }
            return lfm;
        }

        public class LinterError : LinterFileModel.Error
        {
            public Location ErrorLocation;

            /// <summary>
            /// It contains hierarchical information for the convenience of finding errors
            /// </summary>
            public class Location
            {
                public string Class;
                public string Method;
                public string Package;
            }
        }
    }
}