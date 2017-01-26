namespace Linterhub.Cli.Strategy
{
    using System;
    using System.Linq;
    using Runtime;
    using Engine;
    using Engine.Exceptions;
    using System.IO;
    using Engine.Extensions;
    using Newtonsoft.Json;

    public class CatalogStrategy : IStrategy
    {
        public object Run(RunContext context, LinterFactory factory, LogManager log)
        {
            var catalog = GetCatalog(context, factory);
            ProjectConfig config = null;
            if(context.Project != null)
            {
                var projectConfigFile = context.GetProjectConfigPath();
                var existFileConfig = File.Exists(projectConfigFile);
                config = context.GetProjectConfig();
            }
            var result =
                from record in factory.GetRecords().OrderBy(x => x.Name)
                let item = catalog.linters.FirstOrDefault(y => y.name == record.Name)
                let conf_item = config.Linters.FirstOrDefault(y => y.Name == record.Name)
                select new
                {
                    name = record.Name,
                    description = item?.description,
                    languages = item?.languages,
                    active = context.Project != null ? (conf_item?.Active != null ? conf_item?.Active : false) : null
                };

            return result;
        }

        private Linters GetCatalog(RunContext context, LinterFactory factory)
        {
            try
            {
                return context.InputAwailable
                     ? context.Input.DeserializeAsJson<Linters>()
                     : new LinterhubWrapper(context).Info().DeserializeAsJson<Linters>();
            }
            catch (Exception exception)
            {
                throw new LinterParseException(exception);
            }
        }
    }
}