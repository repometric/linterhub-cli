namespace Linterhub.Engine.Linters.coffeelint
{
    public class Warning
    {
        /// <summary>
        /// Contains file path
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// First line of this Warning
        /// </summary>
        public string StartLine { get; set; }

        /// <summary>
        /// Last line of this Warning
        /// </summary>
        public string EndLine { get; set; }

        /// <summary>
        /// Severity of problem (error, warning, etc)
        /// </summary>
        public string Severity { get; set; }

        /// <summary>
        /// Small description of the error
        /// </summary>
        public string Message { get; set; }
    }
}