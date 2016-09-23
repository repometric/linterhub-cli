namespace Metrics.Integrations.Linters.Phpsa
{
    public class LinterError : LinterFileModel.Error
    {
        public string Type { get; set; }
    }

}