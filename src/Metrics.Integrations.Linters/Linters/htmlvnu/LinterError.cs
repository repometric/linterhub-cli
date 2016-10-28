namespace Metrics.Integrations.Linters.htmlvnu
{
    public class LinterError : LinterFileModel.Error
    {
        public string Extract { get; set; }
        public int HiliteStart { get; set; }
        public int HiliteLenght { get; set; }
        public string Type { get; set; }
        public int Character { get; set; }
        public LinterFileModel.Interval Lines { get; set; }
    }
}
