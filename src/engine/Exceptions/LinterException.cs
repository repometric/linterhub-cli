namespace Linterhub.Engine.Exceptions
{
    using System;

    public class LinterException: Exception
    {
        public LinterException(Exception innerException)
            :base(innerException.Message, innerException)
        {
        }

        public LinterException(string message, Exception innerException)
            :base(message, innerException)
        {
        }

        public LinterException(string message)
            :base(message)
        {
        }

        public LinterException(params string[] args)
            :base(string.Join("", args))
        {
        }
    }
}