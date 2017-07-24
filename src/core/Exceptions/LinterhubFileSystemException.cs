namespace Linterhub.Core.Exceptions
{
    public class LinterhubFileSystemException: LinterhubException
    {
        public LinterhubFileSystemException(string name, string filePath)
            : base($"{name} is missing", $"'{name}' was not found: '{filePath}'.", ErrorCode.pathMissing)
        {
        }
    }
}