namespace Linterhub.Engine
{
    public interface ILinterArgs
    {
        string TestPath { get; set; }
    }

    public abstract class LinterArgs : ILinterArgs
    {
        public static readonly ILinterArgs Default = null;

        public abstract string TestPath { get; set; }
    }
}
