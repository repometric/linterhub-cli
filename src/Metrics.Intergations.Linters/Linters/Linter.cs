namespace Metrics.Intergations.Linters
{
    using System.IO;

    public abstract class Linter : ILinter
    {
        public abstract ILinterResult Parse(Stream stream);

        public virtual ILinterResult Parse(Stream stream, ILinterArgs args)
        {
            return Parse(stream);
        }

        public abstract ILinterModel Map(ILinterResult result);
    }
}
