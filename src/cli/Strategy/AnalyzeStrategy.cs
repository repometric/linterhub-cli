namespace Linterhub.Cli.Strategy
{
    using System.Linq;
    using Runtime;
    using Core.Schema;
    using Core.Runtime;
    using Core.Factory;
    using Core.Utils;

    public class AnalyzeStrategy : IStrategy
    {
        public object Run(ServiceLocator locator)
        {
            var context = locator.Get<RunContext>();
            var config = locator.Get<LinterhubConfigSchema>();
            var engineRunner = locator.Get<EngineWrapper>();
            var engineFactory = locator.Get<IEngineFactory>();
            var engines = context.Engines.Count() == 0 ?
                config.Engines.Where(x => x.Active != false)
                .Select(x => x.Name) : context.Engines;
            var ensure = locator.Get<Ensure>();



            ensure.ProjectSpecified();

            var contexts =
                from engine in engines
                let specification = engineFactory.GetSpecification(engine)
                let configOptions = config.Engines.FirstOrDefault(y => y.Name == engine)?.Config ?? specification.Schema.Defaults
                let path = !string.IsNullOrEmpty(context.File) ? context.File : specification.Schema.Defaults.GetValueOrDefault("")
                let runOptions = new EngineOptions
                {
                    { "{path}", /*System.IO.Path.GetFullPath(path)*/ path },
                    { "file://{schema}", context.Linterhub }
                }
                let workingDirectory = context.Directory ?? context.Project
                select new EngineWrapper.Context
                {
                    Specification = specification,
                    ConfigOptions = (EngineOptions)configOptions,
                    RunOptions = runOptions,
                    WorkingDirectory = workingDirectory,
                    Stdin = EngineWrapper.Context.stdinType.NotUse,
                    Locally = context.Locally,
                    Project = context.Project
                };

            return engineRunner.RunAnalyze(contexts.ToList(), new EngineWrapper.RunContext
            {
                File = context.File,
                Directory = context.Directory,
                InputStream = context.Input,
                Project = context.Project
            }, config);
        }
    }
}