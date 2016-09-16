namespace Metrics.Integrations.Linters.Phpcf
{

    public class Warning
    {
        /// <summary>
        ///  Deprications version
        /// </summary>
        public string PhpVersion { get; set; }

        public string FilePath { get; set; }

        public string Line { get; set; }

        /// <summary>
        ///  Advice how to improve code
        /// </summary>
        public string Advice { get; set; }

        /// <summary>
        ///  Description of Warning
        /// </summary>
        public string Description { get; set; }
    }
}
