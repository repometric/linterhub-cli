namespace Metrics.Integrations.Linters
{
    using System;
    using System.IO;
    using System.Reflection;
    using Newtonsoft.Json;
    using System.Linq;
    using Microsoft.Framework.Configuration;

    internal class Program
    {
        // TODO: Isolate settings in class.
        public static string Command { get; set; }
        public static string Linterhub { get; set; }
        public static string Terminal { get; set; }
        public static string TerminalCommand { get; set; }

        internal static void Main(string[] args)
        {
            var configurationBuilder = new ConfigurationBuilder().AddJsonFile(args[0]);
            var configuration = configurationBuilder.Build();
            Command = configuration["command"];
            Linterhub = configuration["linterhub"];
            Terminal = configuration["terminal"];
            TerminalCommand = configuration["terminalCommand"];

            var name = args[1];
            var projectPath = args[2];
            var config = projectPath + "/.linterhub/" + args[1] + ".json";

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
