namespace Metrics.Integrations.Linters.htmlvnu
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Extensions;


    public class Lint : Linter
    {
        public override ILinterResult Parse(Stream stream)
        {
            return new LintResult
            {
                Messages = stream.DeserializeAsJson<List<Message>>()
            };
        }

        public override ILinterModel Map(ILinterResult result)
        {
            var model = new LinterFileModel();
            foreach (var error in ((LintResult)result).Messages)
            {
                var findFile = false;
                var fileError = (LinterFileModel.Error)new LinterError
                {
                    Line = error.LastLine,
                    Lines = new LinterFileModel.Interval
                    {
                        Start = error.FirstLine,
                        End = error.LastLine
                    },
                    Column = new LinterFileModel.Interval()
                    {
                        Start = error.FirstColumn,
                        End = error.LastColumn
                    },
                    Message = error.Msg,
                    Severity = LinterFileModel.Error.SeverityType.error,
                    Extract = error.Extract,
                    HiliteStart = error.HiliteStart,
                    HiliteLenght = error.HiliteLength
                };
                foreach (var file in model.Files.Where(file => file.Path == error.Url))
                {
                    findFile = true;
                    file.Errors.Add(fileError);
                }

                if (!findFile)
                {
                    model.Files.Add(new LinterFileModel.File
                    {
                        Path = error.Url,
                        Errors = new List<LinterFileModel.Error>{ fileError }
                    });
                }
            }

            return model;
        }
    }
}
