namespace Metrics.Integrations.Linters.Phpcf
{

    public class Warning
    {
        public string PhpVersion { get; set; }
        public string FilePath { get; set; }
        public string Line { get; set; }
        public string Advice { get; set; }
        public string Description { get; set; }
    }
}
