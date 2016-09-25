namespace Metrics.Integrations.Linters.htmlhint
{
    /// <summary>
    /// For documentation look at Error class
    /// </summary>
    public class LinterError : LinterFileModel.Error
    {
        public string Type { get; set; }
        public string Raw { get; set; }
    }
}