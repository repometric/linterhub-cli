namespace Linterhub.Engine
{
    public interface ILinterArgs
    {
         string TestPathDocker { get;set;}
    }

    public abstract class LinterArgs : ILinterArgs
    {
        public string TestPathDocker { get; set; }
        public static readonly ILinterArgs Default = null;
    }
}
