namespace Linterhub.Cli.Runtime
{
    using System.IO;

    public class RunContext
    {
        public string Config { get; set; }
        public string Linter { get; set; }
        public string Project { get; set; }
        public bool Activate { get; set; }
        public RunMode Mode { get; set; }
        public Stream Input { get; set; }
        public bool InputAwailable { get; set; }

        public RunContext(RunMode mode = RunMode.Help)
        {
            Mode = mode;
        }

        public Configuration Configuration { get; set; }
    }
}