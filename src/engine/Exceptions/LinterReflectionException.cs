namespace Linterhub.Engine.Exceptions
{
    using System;

    public class LinterReflectionException: LinterException
    {
        public LinterReflectionException(Exception innerException)
            :base(innerException.Message, innerException)
        {
        }
    }
}