namespace Metrics.Integrations.Linters
{
    using System;
    using System.Reflection;

    public class DockerEngine
    {
        public string Build<T>(T configuration, String name, String Filename)
        {
            var propertyInfo = configuration.GetType().GetProperty("TestPath");
            var ProjectPath = (string)propertyInfo.GetValue(configuration, null);
            var args = new ArgBuilder().Build(configuration);
            return string.Format(Program.Command, name, args, Filename, ProjectPath);
        }
    }
}
