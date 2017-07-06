namespace Linterhub.Core.Factory
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Runtime;
    using Core.Schema;

    public class EngineContextFactory
    {
        public IEngineFactory Factory { get; }

        public EngineContextFactory(IEngineFactory factory)
        {
            Factory = factory;
        }

        public EngineWrapper.Context GetContext(string name, string project, LinterhubConfigSchema projectConfig)
        {
            var specification = Factory.GetSpecification(name);
            var configOptions = projectConfig.Engines.FirstOrDefault(y => y.Name == name)?.Config ?? specification.Schema.Defaults;
            var runOptions = new EngineOptions { { "{path}", "" } };
            var workingDirectory = project;
            return new EngineWrapper.Context
            {
                Specification = specification,
                ConfigOptions = (EngineOptions)configOptions,
                RunOptions = runOptions,
                WorkingDirectory = workingDirectory
            };
        }

        public IEnumerable<EngineWrapper.Context> GetContexts(IEnumerable<string> names, string project, LinterhubConfigSchema projectConfig)
        {
            return names.Select(x => GetContext(x, project, projectConfig));
        }
    }
}