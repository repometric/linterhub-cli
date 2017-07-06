namespace Linterhub.Core.Exceptions
{

    public class EngineIsMissingException : LinterhubException
    {

        public EngineIsMissingException(string name)
            : base("Engine is missing", name, ErrorCode.engineMissing)
        {
        }
    }
}