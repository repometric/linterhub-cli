namespace Metrics.Integrations.Linters.coffeelint
{
    public class Warning
    {
        public string path { get; set; }
        public string lineNumber { get; set; }
        public string lineNumberEnd { get; set; }
        public string level { get; set; }
        public string message { get; set; }
    }
}
