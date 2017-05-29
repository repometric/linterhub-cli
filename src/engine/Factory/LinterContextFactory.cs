namespace Linterhub.Engine.Factory
{
    using System.Collections.Generic;
    using System.Linq;
    using Linterhub.Engine.Runtime;
    using Linterhub.Engine.Schema;

    public class LinterContextFactory
    {
        public ILinterFactory Factory { get; }

        public LinterContextFactory(ILinterFactory factory)
        {
            Factory = factory;
        }

        public LinterWrapper.Context GetContext(string name, string project, LinterhubConfigSchema projectConfig)
        {
            var specification = Factory.GetSpecification(name);
            var configOptions = projectConfig.Engines.FirstOrDefault(y => y.Name == name)?.Options ?? specification.Schema.Defaults;
            var runOptions = new LinterOptions { { "{path}", "" } };
            var workingDirectory = project;
            return new LinterWrapper.Context
            {
                Specification = specification,
                ConfigOptions = configOptions,
                RunOptions = runOptions,
                WorkingDirectory = workingDirectory
            };
        }

        public IEnumerable<LinterWrapper.Context> GetContexts(IEnumerable<string> names, string project, LinterhubConfigSchema projectConfig)
        {
            return names.Select(x => GetContext(x, project, projectConfig));
        }
    }
}