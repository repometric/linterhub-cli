namespace Linterhub.Cli.Strategy
{
    using System.Linq;
    using Runtime;
    using Core.Schema;
    using Core.Runtime;
    using Core.Factory;
    using Core.Extensions;

    public class AnalyzeStrategy : IStrategy
    {
        public object Run(ServiceLocator locator)
        {
            var context = locator.Get<RunContext>();
            var config = locator.Get<LinterhubConfigSchema>();
            var engineRunner = locator.Get<EngineWrapper>();
            var engineFactory = locator.Get<IEngineFactory>();
            var engines = AnalyzeStdinStrategy.MergeEngines(context.Engines, config.Engines);
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
                    Stdin = EngineWrapper.Context.stdinType.NotUse
                };

            return new EngineRunner(engineRunner).RunAnalyze(contexts.ToList(), context.Project, context.Directory, context.File, config);
        }
    }
}