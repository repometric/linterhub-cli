namespace Linterhub.Engine
{
    using System.IO;

    public abstract class Linter : ILinter
    {
        public abstract ILinterResult Parse(Stream stream);

        public virtual ILinterResult Parse(Stream stream, ILinterArgs args)
        {
            return Parse(stream);
        }

        public virtual ILinterModel Map(ILinterResult result)
        {
            var map = result as ILinterModel;
            return map;
        }
    }
}
