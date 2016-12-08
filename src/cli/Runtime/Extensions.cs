namespace Linterhub.Cli.Runtime
{
    using System.IO;
    using System.Text;
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
                using (var fileStream = File.Open(projectConfigFile, FileMode.Open))
                {
                    projectConfig = fileStream.DeserializeAsJson<ProjectConfig>();
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
            return string.IsNullOrEmpty(context.Project) ? Directory.GetCurrentDirectory() : context.Project;
        }

        public static Stream GetMemoryStream(this string self)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(self));
        }
    }
}
