namespace Linterhub.Cli.Runtime
{
    using System.IO;
    using System.Linq;
    using Core.Exceptions;
    using Core.Factory;
    using Core.Schema;

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

        public void EngineSpecified()
        {
            if (!Context.Engines.Any())
            {
                throw new EngineException("Engine is not specified");
            }
        }

        public void EngineExists()
        {
            var factory = Locator.Get<IEngineFactory>();
            
            foreach(var engine in Context.Engines)
            {
                if(factory.GetSpecifications().Select(x => x.Schema).Where(x => x.Name == engine).Count() == 0)
                {
                    throw new EngineException("Engine is not exist: " + engine);
                }
            }
        }

        public bool ProjectSpecifiedCheck()
        {
            return Context.Project != Directory.GetCurrentDirectory();
        }

        public void ProjectSpecified()
        {
            if(!ProjectSpecifiedCheck())
            {
                throw new LinterhubException("Project path is missing", "Enter the project path for this task", LinterhubException.ErrorCode.missngParams);
            }
        }

        public void ArgumentSpecified(string name, object value)
        {
            if (value == null)
            {
                throw new LinterhubException($"{name} is missing", $"Argument is not specified: {name.ToLower()}", LinterhubException.ErrorCode.missngParams);
            }
        }

        public void ProjectExists()
        {
            if (!Directory.Exists(Context.Project))
            {
                throw new LinterhubFileSystemException(nameof(Context.Project), Context.Project);
            }
        }

        public void LinterhubExists()
        {
            if (!Directory.Exists(Context.Linterhub))
            {
                throw new LinterhubFileSystemException(nameof(Context.Linterhub), Context.Linterhub);
            }
        }

        public void PlatformConfigExists()
        {
            if (!File.Exists(Context.PlatformConfig))
            {
                throw new LinterhubFileSystemException(nameof(Context.PlatformConfig), Context.PlatformConfig);
            }
        }

        public void DirectoryExists()
        {
            if (!Directory.Exists(Context.Directory))
            {
                throw new LinterhubFileSystemException(nameof(Context.Directory), Context.Directory);
            }
        }

        public void FileExists()
        {
            if (!File.Exists(Context.File))
            {
                throw new LinterhubFileSystemException(nameof(Context.File), Context.File);
            }
        }

        public void ProjectConfigExists()
        {
            if (!File.Exists(Context.ProjectConfig))
            {
                throw new LinterhubFileSystemException(nameof(Context.ProjectConfig), Context.ProjectConfig);
            }
        }
    }
}