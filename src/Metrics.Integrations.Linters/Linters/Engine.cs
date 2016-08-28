namespace Metrics.Integrations.Linters
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Reflection;
    using Extensions;
    using Newtonsoft.Json;

    public class Engine
    {
        public ILinterModel Run(string name, string config)
        {
            var record = Registry.Get().Single(x => x.Name == name);
            var linter = (ILinter)Activator.CreateInstance(record.Linter);
            var args = (ILinterArgs)JsonConvert.DeserializeObject(config, record.Args);
            return Run(linter, args);
        }

        public ILinterModel Run(ILinter linter, ILinterArgs args)
        {
            var argBuilder = new ArgBuilder();
            var cmd = argBuilder.Build(args);
            var wrapper = new CmdWrapper();
            var run = wrapper.RunExecutable(@"C:\WINDOWS\system32\cmd.exe", "/C " + cmd);
            
            // TODO: Introduce interface or read and delete file from cmd
            var output = "";
            var propertyInfo = args.GetType().GetProperty("OutputFile");
            if (propertyInfo != null)
            {
                output = File.ReadAllText((string) propertyInfo.GetValue(args, null));
                File.Delete((string) propertyInfo.GetValue(args, null));
            }
            else
            {
                output = run.Output.ToString();
            }

            // TODO: Read stream from stdout.
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(output)))
            {
                var result = linter.Parse(stream, args);
                var map = linter.Map(result);
                return map;
            }
        }
    }
}
