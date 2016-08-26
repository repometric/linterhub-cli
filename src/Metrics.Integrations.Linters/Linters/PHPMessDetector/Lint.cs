namespace Metrics.Integrations.Linters.Phpmd
{
    using System;
    using Extensions;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using System.IO;
    public class Lint : Linter
    {
        public override ILinterResult Parse(Stream stream)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(LintResult));
            return (LintResult)deserializer.Deserialize(stream);
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
                foreach (var error in file.ViolationsList)
                {
                    LinterError le = new LinterError();
                    le.Row = new LinterFileModel.Interval 
                    { 
                        Start = error.BeginLine, 
                        End = error.EndLine 
                    };
                    le.Message = error.Description;
                    le.Rule = new LinterFileModel.Rule(){
                        Name = error.Rule,
                        Namespace = error.RuleSet
                    };
                    le.ErrorLocation = new LinterError.Location{
                        Class = error.Class,
                        Method = error.Method,
                        Package = error.Package
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
            public class Location
            {
                public string Class;
                public string Method;
                public string Package;
            }
        }
    }
}