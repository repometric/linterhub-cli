namespace Linterhub.Engine.Exceptions
{
    using System;

    public class LinterParseException: LinterException
    {
        public LinterParseException(Exception innerException)
            :base(innerException.Message, innerException)
        {
        }
    }
}