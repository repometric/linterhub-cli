namespace Linterhub.Cli.Strategy
{
    using System.Collections.Generic;
    using System.Linq;
    using Runtime;
    using Linterhub.Engine.Schema;
    using Linterhub.Engine.Runtime;
    using Linterhub.Engine.Factory;
    using Linterhub.Engine.Extensions;

    public class AnalyzeStrategy : IStrategy
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
                let path = !string.IsNullOrEmpty(context.File) ? context.File : specification.Schema.Defaults.GetValueOrDefault("")
                let runOptions = new LinterOptions
                {
                    { "{path}", /*System.IO.Path.GetFullPath(path)*/ path },
                    { "file://{schema}", context.Linterhub }
                }
                let workingDirectory = context.Directory ?? context.Project
                select new LinterWrapper.Context
                {
                    Specification = specification,
                    ConfigOptions = (LinterOptions)configOptions,
                    RunOptions = runOptions,
                    WorkingDirectory = workingDirectory,
                    Stdin = LinterWrapper.Context.stdinType.NotUse
                };

            var r = linterRunner.RunAnalysis(contexts.First());
            var t = r.DeserializeAsJson<EngineOutputSchema.ResultType[]>();

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

            var tl = t.ToList();
            tl.Sort((a, b) => a.Path.CompareTo(b.Path));
            return tl;
            /* 
            var linterResults = new List<RunResult>();
            try
            {
                Stream input;

                if (!context.InputAwailable)
                {
                    var configs = GetConfigs(context, factory, log);
                    var linterModels = new List<LinterResult>();
                    linterModels.AddRange(
                        configs
                            .Linters.Where(x => x.Command != null)
                            .Select(linterConfig => new LinterResult
                            {
                                Name = linterConfig.Name,
                                Task = Task<string>.Factory.StartNew(() =>
                                    new LinterhubWrapper(context).Analyze(linterConfig.Name, linterConfig.Command, context.Project)
                                   )
                            }
                            ));

                    Task.WaitAll(linterModels.Select(x => x.Task).ToArray());
                    foreach (var linter in linterModels)
                    {
                        using (input = linter.Task.Result.GetMemoryStream())
                        {
                            linterResults.Add(ExcludeWarnings(new RunResult
                            {
                                Name = linter.Name,
                                Model = factory.CreateModel(linter.Name, input, null)

                            }, configs.Linters.First(x => x.Name == linter.Name).Ignore, configs.Ignore));
                        }
                    }
                }
                else
                {
                    input = context.Input;
                    linterResults.Add(new RunResult
                    {
                        Name = context.Linter,
                        Model = factory.CreateModel(context.Linter, input, null)
                    });
                }
            }
            catch (Exception exception)
            {
                throw new LinterEngineException(exception.Message, exception);
            }
            return linterResults;
        }

        private RunResult ExcludeWarnings(RunResult result, List<ProjectConfig.IgnoreRule> rules, List<ProjectConfig.IgnoreRule> global)
        {
            rules.AddRange(global);
            if (result.Model is LinterFileModel)
            {
                LinterFileModel model = result.Model as LinterFileModel;
                rules.ForEach(rule =>
                {
                    model.Files.ToList().ForEach(file =>
                    {
                        file.Errors = file.Errors.Where(x =>
                        {
                            bool filename = rule.FileName != null ? file.Path == rule.FileName : true;
                            bool line = rule.Line != null ? x.Line == rule.Line.ToString() : true;
                            bool error = rule.Error != null ? x.Rule.Name == rule.Error : true;
                            return !(filename && line && error);
                        }
                        ).ToList();
                    });
                });
                result.Model = model;
            }
            return result;
        }

        private ProjectConfig GetConfigs(RunContext context, LinterFactory factory, LogManager log)
        {
            var validateResult = context.ValidateContext(factory, log);
            return validateResult.IsLinterSpecified
                    ? GetLinter(factory, context.Linter, validateResult) :
                        GetLinters(factory, validateResult);
        }

        public ProjectConfig GetLinters(LinterFactory factory, ValidationContext validateContext)
        {
            foreach (var linter in validateContext.ProjectConfig.Linters.Where(x => x.Active != false))
            {
                linter.Command = linter.Command ?? GetCommand(factory, validateContext, linter);
            }
            return validateContext.ProjectConfig;
        }

        public ProjectConfig GetLinter(LinterFactory factory, string name, ValidationContext validateContext)
        {
            var linter = validateContext.ProjectConfig.Linters.FirstOrDefault(x => x.Name == name && x.Active != false);
            var command = GetCommand(factory, validateContext, linter, name);
            if (linter == null)
            {
                validateContext.ProjectConfig.Linters.Add(new ProjectConfig.Linter
                {
                    Command = command,
                    Name = name,
                });
            }
            else
            {
                linter.Command = linter.Command ?? command;
            }
            return validateContext.ProjectConfig;
        }

        public string GetCommand(LinterFactory factory, ValidationContext validateConext, ProjectConfig.Linter linter = null, string linterName = null)
        {
            linter = linter ?? new ProjectConfig.Linter();

            return linter.Command ?? (
                        linter.Active != false ?
                            factory.BuildCommand(
                                linterName ?? linter.Name,
                                validateConext.WorkDir,
                                validateConext.Path,
                                validateConext.ArgMode) : null);
        }*/
        }
    }
}