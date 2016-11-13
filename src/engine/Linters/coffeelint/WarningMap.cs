namespace Linterhub.Engine.Linters.coffeelint
{
    using CsvHelper.Configuration;

    public sealed class WarningMap : CsvClassMap<Warning>
    {
        public WarningMap()
        {
            Map(m => m.EndLine).Name("lineNumberEnd");
            Map(m => m.Path).Name("path");
            Map(m => m.Message).Name("message");
            Map(m => m.StartLine).Name("lineNumber");
            Map(m => m.Severity).Name("level");
        }
    }
}
