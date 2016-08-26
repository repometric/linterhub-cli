namespace Metrics.Integrations.Linters
{
    using System.IO;

    public interface ILinter
    {
        ILinterResult Parse(Stream stream);

        ILinterResult Parse(Stream stream, ILinterArgs args);

        ILinterModel Map(ILinterResult result);
    }
}
