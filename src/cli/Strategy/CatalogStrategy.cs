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
        public object Run(RunContext context, LinterEngine engine, LogManager log)
        {
            var catalog = GetCatalog(context, engine);
            var result =
                from record in engine.Factory.GetRecords().OrderBy(x => x.Name)
                let item = catalog.linters.FirstOrDefault(y => y.name == record.Name)
                select new
                {
                    name = record.Name,
                    description = item?.description,
                    languages = item?.languages
                };

            return result;
        }

        private Linters GetCatalog(RunContext context, LinterEngine engine)
        {
            try
            {
                return context.InputAwailable
                     ? context.Input.DeserializeAsJson<Linters>()
                     : new LinterhubWrapper(context, engine).Info().DeserializeAsJson<Linters>();
            }
            catch (Exception exception)
            {
                throw new LinterParseException(exception);
            }
        }
    }
}