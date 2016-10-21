using System.Linq;
using Metrics.Integrations.Extensions;

namespace Metrics.Integrations.Linters.eslint
{
    using System;
    using System.IO;
    using System.Text;
    using System.Collections.Generic;
    using Newtonsoft.Json.Linq;
    using System.Linq;

    public class Lint : Linter
    {
        /// <summary>
        /// Parse JsLint (json) to Files Error Model
        /// </summary>
        /// <param name="model">JsLint model</param>
        /// <returns>Files model</returns>
        //public static FilesModel ParseModel(jslint model)
        //{
        //    var filesModel = new FilesModel();
        //    foreach (var file in model.file)
        //    {
        //        var fileItem = new FileModel() { Path = file.name };
        //        foreach (var filerError in file.issue.Select(issue => new EsLintFileError()
        //        {
        //            Character = issue.@char,
        //            Evidens = issue.evidence,
        //            Line = issue.line,
        //            Message = issue.reason
        //        }))
        //        {
        //            fileItem.FileErrors.Add(filerError);
        //        }
        //        filesModel.Files.Add(fileItem);
        //    }
        //    return filesModel;
        //}

        public override ILinterResult Parse(Stream stream)
        {
            return new LintResult
            {
                FilesList = stream.DeserializeAsJson<List<File>>()
            };
        }

        public override ILinterModel Map(ILinterResult result)
        {
            var model = new LinterFileModel
            {
                Files = ((LintResult)result).FilesList.Select(file => new LinterFileModel.File
                {
                    Path = file.FilePath,
                    Errors = file.Messages.Select(fileError => new LinterFileModel.Error
                    {
                        Line = fileError.Line,
                        Message = fileError.MessageError
                    }).ToList()
                }).ToList()
            };

            return model;
        }
    }
}