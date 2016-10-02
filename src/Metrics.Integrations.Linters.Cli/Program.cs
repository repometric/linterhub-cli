namespace Metrics.Integrations.Linters
{
    using System;
    using System.IO;
    using Newtonsoft.Json;

    internal class Program
    {
        internal static void Main(string[] args)
        {
            var name = args[0];
            var config = args.Length > 1 
                       ? args[1]
                       : "../../.linterhub/" + name + "/config.json";

            var engine = new Engine();
            var result = engine.Run(name, File.ReadAllText(config));
            //engine.Run(new Linter(), new LinterArgs());
            Console.WriteLine(JsonConvert.SerializeObject(result));
            Console.ReadLine();
        }
    }
}
