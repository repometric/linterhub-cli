namespace Metrics.Integrations.Linters
{
    using System;
    using System.IO;
    using Newtonsoft.Json;

    internal class Program
    {
        internal static void Main(string[] args)
        {
            var engine = new Engine();
            var result = engine.Run(args[0], File.ReadAllText(args[1]));
            //engine.Run(new Linter(), new LinterArgs());
            Console.WriteLine(JsonConvert.SerializeObject(result));
            Console.ReadLine();
        }
    }
}
