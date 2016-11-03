namespace Linterhub.Engine.Linters.pep8
{
    public class Warning
    {
        public string FilePath { get; set; }

        public string Line { get; set; }

        /// <summary>
        ///  For more information:
        ///  http://pep8.readthedocs.io/en/release-1.7.x/intro.html#error-codes
        /// </summary>
        public string Pattern { get; set; }

        /// <summary>
        ///  Description of Warning
        /// </summary>
        public string Description { get; set; }
    }
}
