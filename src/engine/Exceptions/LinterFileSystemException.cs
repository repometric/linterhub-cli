namespace Linterhub.Engine.Exceptions
{
    public class LinterFileSystemException: LinterException
    {
        public LinterFileSystemException(string name, string filePath)
            :base($"{name}: {filePath}")
        {
        }
    }
}