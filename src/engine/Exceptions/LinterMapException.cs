namespace Linterhub.Engine.Exceptions
{
    using System;

    public class LinterMapException: LinterException
    {
        public LinterMapException(Exception innerException)
            :base(innerException.Message, innerException)
        {
        }
    }
}