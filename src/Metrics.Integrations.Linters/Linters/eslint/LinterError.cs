namespace Metrics.Integrations.Linters.eslint
{
    public class LinterError : LinterFileModel.Error
    {
        public string NodeType { get; set; }
    }

}
