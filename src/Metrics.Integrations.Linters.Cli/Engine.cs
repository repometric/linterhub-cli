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
            return Run(linter, args, name);
        }

        public ILinterModel Run(ILinter linter, ILinterArgs args, String name)
        {
            var DockerBuilder = new DockerEngine();
            var FileName = Guid.NewGuid() + ".txt";
            var cmd = DockerBuilder.Build(args, name, FileName);
            var wrapper = new CmdWrapper();
            string env_cmd = Environment.GetEnvironmentVariable("ComSpec");
            var run = wrapper.RunExecutable(env_cmd, "/C sh " + cmd, DockerBuilder.LinterHubPath);
            // TODO: Introduce interface or read and delete file from cmd
            var output = "";
            FileName = DockerBuilder.LinterHubPath + FileName;


            var log_path = args.TestPath + "\\.linterhub_logs";
            if (Directory.Exists(log_path))
            {
                var time = (int)(DateTime.UtcNow.Ticks / 1000);
                File.WriteAllText(log_path + "\\" + time +"_linterhub_output" + ".txt", run.Output.ToString());
                File.WriteAllText(log_path + "\\" + time + "_linterhub_error" + ".txt", run.Error.ToString());
            }

            output = File.ReadAllText(FileName);
            File.Delete(FileName);

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
