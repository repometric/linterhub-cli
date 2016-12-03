namespace Linterhub.Engine
{
    public interface ILinterArgs
    {

    }

    public abstract class LinterArgs : ILinterArgs
    {
        public static readonly ILinterArgs Default = null;
    }
}
