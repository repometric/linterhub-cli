namespace Metrics.Integrations.Linters
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using Newtonsoft.Json;
    using System.Linq;

    internal class Program
    {
        internal static void Main(string[] args)
        {
            var name = args[0];
            var projectPath = args[1];
            var config = projectPath + "/.linterhub/" + args[0] + ".json";

            if(!File.Exists(config))
            {
                var path = ((string)System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().CodeBase)).ToString().Substring(6) + "\\..\\configs\\" + args[0] + ".json";
                var file = File.ReadAllText(path);
                var j_config2 = JsonConvert.DeserializeObject(file, Registry.Get().Single(x => x.Name == name).Args);
                var propertyInfo = j_config2.GetType().GetProperty("TestPath");
                propertyInfo.SetValue(j_config2, projectPath);
                File.AppendAllText(config, JsonConvert.SerializeObject(j_config2));
            }
            if (args.Length <= 2)
            {
                var engine = new Engine();
                var result = engine.Run(name, File.ReadAllText(config));
                Console.WriteLine(JsonConvert.SerializeObject(result));
            }
        }
    }
}
