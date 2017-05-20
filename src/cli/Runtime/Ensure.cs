namespace Linterhub.Cli.Runtime
{
    using System.IO;
    using System.Linq;
    using Linterhub.Engine.Exceptions;
    using Linterhub.Engine.Schema;

    public class Ensure
    {
        protected ServiceLocator Locator { get; }

        protected RunContext Context { get; }

        protected LinterhubConfigSchema Schema { get; }

        public Ensure(ServiceLocator locator)
        {
            Locator = locator;
            Context = locator.Get<RunContext>();
            //Schema = locator.Get<LinterhubSchema>();
        }

        public void LinterSpecified()
        {
            if (!Context.Linters.Any())
            {
                throw new LinterEngineException("Linter is not specified");
            }
        }

        public void LinterExists()
        {
            foreach (var linter in Context.Linters)
            {
                if (!Locator.Get<LinterhubConfigSchema>().Engines.Any(x => x.Name == linter))
                {
                    //throw new LinterEngineException("Linter is not exist: " + linter);
                }
            }
        }

        public void ArgumentSpecified(string name, object value)
        {
            if (value == null)
            {
                throw new LinterEngineException($"Argument is not specified: {name.ToLower()}");
            }
        }

        public void ProjectExists()
        {
            if (!Directory.Exists(Context.Project))
            {
                throw new LinterFileSystemException(nameof(Context.Project), Context.Project);
            }
        }

        public void LinterhubExists()
        {
            if (!Directory.Exists(Context.Linterhub))
            {
                throw new LinterFileSystemException(nameof(Context.Linterhub), Context.Linterhub);
            }
        }

        public void PlatformConfigExists()
        {
            if (!File.Exists(Context.PlatformConfig))
            {
                throw new LinterFileSystemException(nameof(Context.PlatformConfig), Context.PlatformConfig);
            }
        }

        public void DirectoryExists()
        {
            if (!Directory.Exists(Context.Directory))
            {
                throw new LinterFileSystemException(nameof(Context.Directory), Context.Directory);
            }
        }

        public void FileExists()
        {
            if (!File.Exists(Context.File))
            {
                throw new LinterFileSystemException(nameof(Context.File), Context.File);
            }
        }

        public void ProjectConfigExists()
        {
            if (!File.Exists(Context.ProjectConfig))
            {
                throw new LinterFileSystemException(nameof(Context.ProjectConfig), Context.ProjectConfig);
            }
        }
    }
}