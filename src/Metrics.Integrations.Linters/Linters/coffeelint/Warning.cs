namespace Metrics.Integrations.Linters.coffeelint
{
    public class Warning
    {
        public string Path { get; set; }
        public string StartLine { get; set; }
        public string EndLine { get; set; }
        public string Severity { get; set; }
        public string Message { get; set; }
    }
}