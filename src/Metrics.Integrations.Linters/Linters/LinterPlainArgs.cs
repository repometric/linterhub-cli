namespace Metrics.Integrations.Linters
{
    public class LinterPlainArgs : ILinterArgs, IArgProvider
    {
        [Arg(separator: "")]
        public string Cmd { get; set; }

        public string TestPath { get; set; }

        public string Path { get; set; }

        public string Build()
        {
            return string.Format(Cmd, Path);
        }
    }
}
