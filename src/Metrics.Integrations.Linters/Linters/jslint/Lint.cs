namespace Metrics.Integrations.Linters.jslint
{
    using System;
    using System.IO;
    using System.Text;
    using System.Collections.Generic;
    using Newtonsoft.Json.Linq;
    using System.Linq;

    public class Lint : Linter
    {
        public IEnumerable<string> ReadLines(Func<Stream> streamProvider, Encoding encoding)
        {
            using (var stream = streamProvider())
            using (var reader = new StreamReader(stream, encoding))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }

        public override ILinterResult Parse(Stream stream)
        {
            var result = new LintResult() { FilesList = new List<File>()};
            foreach (var line in ReadLines(() => stream, Encoding.UTF8))
            {
                var jsonFileLine = JArray.Parse(line);
                var path = jsonFileLine[0].ToString();
                var errors = jsonFileLine[1].Children<JObject>().Select(x => x.ToObject<Error>()).ToList();
                result.FilesList.Add(new File
                {
                    Path = path,
                    Errors = errors
                });
            }

            return result;
        }

        public override ILinterModel Map(ILinterResult result)
        {
            var model = new LinterFileModel
            {
                Files = ((LintResult) result).FilesList.Select(file => new LinterFileModel.File
                {
                    Path = file.Path,
                    Errors = file.Errors.Select(fileError => (LinterFileModel.Error)new JsLintError
                    {
                        A = fileError.A,
                        Character = fileError.Character,
                        Code = fileError.Code,
                        Evidence = fileError.Evidence,
                        Id = fileError.Id,
                        Line = fileError.Line,
                        Raw = fileError.Raw,
                        Message = fileError.Reason
                    }).ToList()
                }).ToList()
            };

            return model;
        }
    }
}