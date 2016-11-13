namespace Linterhub.Engine
{
    using System;
    using System.IO; 
    using Exceptions;

    public class LinterProxy : ILinter
    {
        public ILinter Target { get; }

        public LinterProxy(ILinter linter)
        {
            Target = linter;
        }

        public ILinterModel Map(ILinterResult result)
        {
            try
            {
                return Target.Map(result);
            }
            catch (Exception exception)
            {
                throw new LinterMapException(exception);
            }
        }

        public ILinterResult Parse(Stream stream, ILinterArgs args)
        {
            try
            {
                return Target.Parse(stream, args);
            }
            catch (Exception exception)
            {
                throw new LinterParseException(exception);
            }
        }
    }
}