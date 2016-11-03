namespace Linterhub.Engine.Exceptions
{
    using System;

    public class LinterNotFoundException: LinterException
    {
        public LinterNotFoundException(string name)
            :base(name)
        {
        }
    }
}