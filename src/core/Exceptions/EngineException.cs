namespace Linterhub.Core.Exceptions
{
    public class EngineException: LinterhubException
    {

        public EngineException(string title, string message)
            : base(title, message, ErrorCode.engineCrashed, null)
        {
        }

        public EngineException(string title)
            : base(title, null, ErrorCode.engineCrashed, null)
        {
        }
    }
}