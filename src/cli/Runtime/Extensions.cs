namespace Linterhub.Cli.Runtime
{
    using System;
    using System.IO;
    using System.Text;
    using Engine.Exceptions;
    using Engine.Extensions;

    public static class Extensions
    {
        public static string GetProjectConfigPath(this RunContext context)
        {
            return Path.Combine(context.Project, ".linterhub.json");
        }

        public static ProjectConfig GetProjectConfig(this RunContext context)
        {
            ProjectConfig projectConfig;
            var projectConfigFile = context.GetProjectConfigPath();

            if (!File.Exists(projectConfigFile))
            {
                projectConfig = new ProjectConfig();
            }
            else
            {
                try
                {
                    using (var fileStream = File.Open(projectConfigFile, FileMode.Open))
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

        public static string SetProjectConfig(this RunContext context, ProjectConfig projectConfig)
        {
            var content = projectConfig.SerializeAsJson();
            File.WriteAllText(context.GetProjectConfigPath(), content);
            return content;
        }

        public static string GetProjectPath(this RunContext context)
        {
            var path = string.IsNullOrEmpty(context.Project) ? Directory.GetCurrentDirectory() : context.Project;
            return path;
        }

        public static string GetAnalyzePath(this RunContext context)
        {
            return string.IsNullOrEmpty(context.File) ? context.Dir : context.File;
        }

        public static Stream GetMemoryStream(this string self)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(self));
        }
    }
}
