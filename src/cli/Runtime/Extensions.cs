namespace Linterhub.Cli.Runtime
{
    using System;
    using System.IO;
    using System.Text;
    using System.Linq;
    using Engine;
    using Engine.Exceptions;
    using Engine.Extensions;

    public static class Extensions
    {
        public static ValidationContext ValidateContext(this RunContext context, LinterFactory factory, LogManager log)
        {
            var validation = new ValidationContext
            {
                WorkDir = context.Project,
                PathFileConfig = Path.Combine(context.Project, ".linterhub.json"),
                Path = string.IsNullOrEmpty(context.File) ? context.Dir : context.File,
                ArgMode = string.IsNullOrEmpty(context.File) ? ArgMode.Folder : ArgMode.File,
                IsLinterSpecified = !string.IsNullOrEmpty(context.Linter)
            };
            validation.ExistFileConfig = File.Exists(validation.PathFileConfig);
            validation.ProjectConfig = context.GetProjectConfig(factory, validation);

            log.Trace("Expected project config: " + validation.PathFileConfig);
            return validation;
        }
        
        public static ProjectConfig GetProjectConfig(this RunContext context, LinterFactory factory, ValidationContext validationContext)
        {
            ProjectConfig projectConfig;
            if (!validationContext.ExistFileConfig)
            {
                projectConfig = new ProjectConfig();
                foreach (var linter in factory.GetRecords().Where(x => x.ArgsDefault))
                {
                    projectConfig.Linters.Add(new ProjectConfig.Linter
                    {
                        Name = linter.Name,
                    });
                }
            }
            else
            {
                try
                {
                    using (var fileStream = File.Open(validationContext.PathFileConfig, FileMode.Open))
                    {
                        projectConfig = fileStream.DeserializeAsJson<ProjectConfig>();
                    }
                }
                catch (Exception exception)
                {
                    throw new LinterEngineException("Error parsing project configuration file", exception);
                }
            }
            
            return projectConfig;
        }

        public static string SetProjectConfig(this RunContext context, ValidationContext validationContext)
        {
            var content = validationContext.ProjectConfig.SerializeAsJson();
            File.WriteAllText(validationContext.PathFileConfig, content);
            return content;
        }

        public static string GetProjectPath(this RunContext context)
        {
            var path = string.IsNullOrEmpty(context.Project) ? Directory.GetCurrentDirectory() : context.Project;
            return path;
        }

        public static Stream GetMemoryStream(this string self)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(self));
        }
    }
}
