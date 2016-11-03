namespace Linterhub.Cli.Runtime
{
    using System;
    using System.IO;

    public class LinterContext
    {
        public Configuration Configuration { get; }
        public RunContext RunContext { get; }
        public string OutputFileName { get; }
        public string OutputFullPath { get; }
        public string Stamp { get; }

        public LinterContext(Configuration config, RunContext context)
        {
            Stamp = DateTime.UtcNow.ToString(Extensions.LogFormat);
            OutputFileName = Guid.NewGuid() + Extensions.LogExtension;
            OutputFullPath = Path.Combine(config.Linterhub, OutputFileName);
            this.Configuration = config;
            this.RunContext = context;
        }
    }
}