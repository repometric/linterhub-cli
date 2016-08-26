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
                LinterFileModel.File lf = new LinterFileModel.File();
                lf.Path = file.FileName;
                foreach (var error in file.ViolationsList)
                {
                    LinterError le = new LinterError();
                    LinterFileModel.Interval li = new LinterFileModel.Interval();
                    li.Start = error.BeginLine;
                    li.End = error.EndLine;
                    le.Row = li;
                    le.Message = error.Description;
                    LinterFileModel.Rule lr = new LinterFileModel.Rule();
                    lr.Name = error.Rule;
                    lr.Namespace = error.RuleSet;
                    le.Rule = lr;
                    var loc = new LinterError.Location();
                    loc.Class = error.Class;
                    loc.Method = error.Method;
                    loc.Package = error.Package;
                    le.ErrorLocation = loc;
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