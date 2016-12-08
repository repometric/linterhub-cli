namespace Linterhub.Cli.Strategy
{
    using System;
    using System.Linq;
    using Runtime;
    using Engine;
    using Engine.Exceptions;
    using Engine.Extensions;

    public class CatalogStrategy : IStrategy
    {
        public object Run(RunContext context, LinterFactory factory, LogManager log)
        {
            var catalog = GetCatalog(context, factory);
            var result =
                from record in factory.GetRecords().OrderBy(x => x.Name)
                let item = catalog.linters.FirstOrDefault(y => y.name == record.Name)
                select new
                {
                    name = record.Name,
                    description = item?.description,
                    languages = item?.languages
                };

            return result;
        }

        private Linters GetCatalog(RunContext context, LinterFactory factory)
        {
            try
            {
                return context.InputAwailable
                     ? context.Input.DeserializeAsJson<Linters>()
                     : new LinterhubWrapper(context, factory).Info().DeserializeAsJson<Linters>();
            }
            catch (Exception exception)
            {
                throw new LinterParseException(exception);
            }
        }
    }
}