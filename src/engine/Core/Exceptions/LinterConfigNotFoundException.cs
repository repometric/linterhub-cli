namespace Linterhub.Engine.Exceptions
{
    public class LinterConfigNotFoundException: LinterException
    {
        public LinterConfigNotFoundException(string project)
            :base("Linterhub config was not found: " + project)
        {
        }
    }
}