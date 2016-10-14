namespace Metrics.Integrations.Linters.Runtime
{
    public class RunContext
    {
        public string Configuration { get; set; }
        public string Linter { get; set; }
        public string Project { get; set; }
        public ModeEnum Mode { get; set; }

        public RunContext(ModeEnum mode = ModeEnum.Analyze)
        {
            Mode = mode;
        }

        public enum ModeEnum
        {
            Analyze,
            Linters,
            Generate,
            Help
        }
    }
}