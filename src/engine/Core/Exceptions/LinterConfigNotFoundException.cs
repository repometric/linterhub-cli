namespace Linterhub.Engine.Exceptions
{
    using System;

    public class LinterConfigNotFoundException: LinterException
    {
        public LinterConfigNotFoundException(string project)
            :base("Linterhub config was not found: " + project)
        {
        }
    }
}