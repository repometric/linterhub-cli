namespace Linterhub.Engine.Exceptions
{

    public class LinterNotFoundException: LinterException
    {
        public LinterNotFoundException(string name)
            :base("Linter was not found: " + name)
        {
        }
    }
}