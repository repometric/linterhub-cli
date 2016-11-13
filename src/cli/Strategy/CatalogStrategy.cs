namespace Linterhub.Cli.Strategy
{
    using System;
    using System.IO;
    using System.Linq;
    using Linterhub.Cli.Runtime;
    using Linterhub.Engine;
    using Linterhub.Engine.Exceptions;
    using Linterhub.Engine.Extensions;
    using Linterhub.Engine.Linters;

    public class CatalogStrategy : IStrategy
    {
        public object Run(RunContext context, LinterEngine engine, LogManager log)
        {
            var catalog = GetCatalog(context, engine);
            var result = engine.Factory.GetRecords().Select(x => new 
            {
                name = x.Name,
                languages = catalog.linters.FirstOrDefault(y => y.name == x.Name)?.languages
            }).OrderBy(x => x.name);

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