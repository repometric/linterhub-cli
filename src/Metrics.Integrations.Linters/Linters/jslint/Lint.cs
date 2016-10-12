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
                    path = path,
                    error = errors
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
                    Path = file.path,
                    Errors = file.error.Select(fileError => (LinterFileModel.Error)new JsLintError
                    {
                        A = fileError.a,
                        Character = fileError.character,
                        Code = fileError.code,
                        Evidence = fileError.evidence,
                        Id = fileError.id,
                        Line = fileError.line,
                        Raw = fileError.raw,
                        Message = fileError.reason
                    }).ToList()
                }).ToList()
            };

            return model;
        }
    }
}