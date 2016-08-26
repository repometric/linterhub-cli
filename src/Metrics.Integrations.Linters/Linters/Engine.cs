namespace Metrics.Integrations.Linters
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
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
            var run = wrapper.RunExecutable("cmd.exe", cmd);
            // TODO: Read stream from stdout.
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(run.Output.ToString())))
            {
                var result = linter.Parse(stream, args);
                var map = linter.Map(result);
                return map;
            }
        }
    }
}
