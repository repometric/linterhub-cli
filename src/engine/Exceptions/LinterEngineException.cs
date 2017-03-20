namespace Linterhub.Engine.Exceptions
{
    using System;

    public class LinterEngineException: LinterException
    {
        public LinterEngineException(Exception innerException)
            :base(innerException.Message, innerException)
        {
        }

        public LinterEngineException(string message, Exception innerException)
            :base(message, innerException)
        {
        }

        public LinterEngineException(string message)
            :base(message)
        {
        }
    }
}