namespace Linterhub.Cli.Runtime
{
    using System.IO;
    using Engine.Extensions;

    public static class Extensions
    {
        public static string GetProjectConfigPath(this RunContext context)
        {
            return Path.Combine(context.Project, ".linterhub.json");
        }

        public static ExtConfig GetProjectConfig(this RunContext context)
        {
            ExtConfig extConfig;
            var projectConfigFile = context.GetProjectConfigPath();

            if (!File.Exists(projectConfigFile))
            {
                extConfig = new ExtConfig();
            }
            else
            {
                using (var fileStream = File.Open(projectConfigFile, FileMode.Open))
                {
                    extConfig = fileStream.DeserializeAsJson<ExtConfig>();
                }
            }

            return extConfig;
        }

        public static string SetProjectConfig(this RunContext context, ExtConfig extConfig)
        {
            var content = extConfig.SerializeAsJson();
            File.WriteAllText(context.GetProjectConfigPath(), content);
            return content;
        }
    }
}
