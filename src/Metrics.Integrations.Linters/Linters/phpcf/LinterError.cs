namespace Metrics.Integrations.Linters.Phpcf
{
    public class LinterError : LinterFileModel.Error
    {
        public string PhpVersion { get; set; }

        /// <summary>
        ///  Advice how to improve code
        /// </summary>
        public string Advice { get; set; }
    }

}