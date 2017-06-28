namespace Linterhub.Cli.Strategy
{
    using System.Collections.Generic;
    using System.Linq;
    using Runtime;
    using Linterhub.Engine.Schema;
    using Linterhub.Engine.Runtime;
    using Linterhub.Engine.Factory;
    using Linterhub.Engine.Extensions;
    using System.IO;

    public class AnalyzeStdinStrategy : IStrategy
    {
        private IEnumerable<string> MergeLinters(IEnumerable<string> lintersFromCommand, IEnumerable<LinterhubConfigSchema.ConfigurationType> lintersFromConfig)
        {
            var linters = lintersFromConfig
                .Where(x => x.Active != false)
                .Select(x => x.Name)
                .Concat(lintersFromCommand)
                .Distinct();
            return linters;
        }

        public object Run(ServiceLocator locator)
        {
            var context = locator.Get<RunContext>();
            var config = locator.Get<LinterhubConfigSchema>();
            var linterRunner = locator.Get<LinterWrapper>();
            var linterFactory = locator.Get<ILinterFactory>();
            var linters = MergeLinters(context.Linters, config.Engines);

            var contexts =
                from linter in linters
                let specification = linterFactory.GetSpecification(linter)
                let configOptions = config.Engines.FirstOrDefault(y => y.Name == linter)?.Config ?? specification.Schema.Defaults
                let path = !string.IsNullOrEmpty(context.File) ? context.File : null
                let fileName = path ?? "#stdin"
                let runOptions = new LinterOptions
                {
                    { "{path}", /*System.IO.Path.GetFullPath(path)*/ path },
                    { "file://{schema}", context.Linterhub },
                    { "file://{stdin}", fileName },
                }
                let workingDirectory = context.Directory ?? context.Project
                select new LinterWrapper.Context
                {
                    Specification = specification,
                    ConfigOptions = (LinterOptions)configOptions,
                    RunOptions = runOptions,
                    WorkingDirectory = workingDirectory,
                    Stdin = (specification.Schema.Stdin.Available ?? false)
                            ? LinterWrapper.Context.stdinType.UseWithLinter : LinterWrapper.Context.stdinType.Use
                };

            var t = new LinterRunner(linterRunner).RunAnalyze(contexts.ToList());

            foreach (var file in t)
            {
                file.Path = file
                    .Path
                    .Replace(context.Project, string.Empty)
                    .Replace(System.IO.Path.GetFullPath(context.Project), string.Empty)
                    .TrimStart('/')
                    .TrimStart('\\')
                    .Replace("/", "\\");
            }

            t.Sort((a, b) => a.Path.CompareTo(b.Path));
            return t;
        }
    }
}