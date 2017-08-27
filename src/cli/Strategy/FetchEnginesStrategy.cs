namespace Linterhub.Cli.Strategy
{
    using System.Linq;
    using Runtime;
    using Core.Schema;
    using Core.Factory;
    using Core.Utils;
    using System.IO;

    public class FetchEnginesStrategy : IStrategy
    {
        public object Run(ServiceLocator locator)
        {
            var context = locator.Get<RunContext>();
            var config = locator.Get<LinterhubConfigSchema>();
            var engineFactory = locator.Get<IEngineFactory>();
            var ensure = locator.Get<Ensure>();

            ensure.ProjectSpecified();

            var result = new LinterhubFetchSchema();

            var engines = engineFactory.GetSpecifications().Select(x => x.Schema).OrderBy(x => x.Name).ToList();

            engines.ForEach(engine => {
                if (engine.Extensions.Select(x =>
                {
                    return Directory.GetFiles(context.Project, x, SearchOption.AllDirectories)
                        .Where(y => !config.Ignore.Exists(z => y.Contains(z.Mask))).ToList();
                }).Where(x => x.Any()).Any())
                {
                    result.Add(new LinterhubFetchSchema.ResultType()
                    {
                        Name = engine.Name,
                        Found = LinterhubFetchSchema.ResultType.FoundType.sourceExtension
                    });
                }

                engine.Configs.ForEach(file =>
                {
                    if (File.Exists(Path.Combine(context.Project, file)))
                    {
                        result.Remove(result.Find(x => x.Name == engine.Name));
                        result.Add(new LinterhubFetchSchema.ResultType()
                        {
                            Name = engine.Name,
                            Found = LinterhubFetchSchema.ResultType.FoundType.engineConfig
                        });
                    }
                });
            });

            // Search in npm package config
            var configPath = Path.Combine(context.Project, "package.json");
            if (File.Exists(configPath))
            {
                var npmConfig = File.ReadAllText(configPath).DeserializeAsJson<NpmPackageSchema>();
                engines.ForEach(engine =>
                {
                    if (npmConfig.Dependencies.ContainsKey(engine.Name) || 
                        npmConfig.DevDependencies.ContainsKey(engine.Name))
                    {
                        result.Remove(result.Find(x => x.Name == engine.Name));
                        result.Add(new LinterhubFetchSchema.ResultType()
                        {
                            Name = engine.Name,
                            Found = LinterhubFetchSchema.ResultType.FoundType.projectConfig
                        });
                    }
                });
            }

            return result.Where(y => !config.Engines.Exists(x => x.Name == y.Name && (x.Active ?? false)));
        }
    }
}