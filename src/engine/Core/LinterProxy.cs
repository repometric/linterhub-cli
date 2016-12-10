namespace Linterhub.Engine
{
    using System;
    using System.IO; 
    using Exceptions;

    /// <summary>
    /// Represents linter which wraps all exceptions during parsing and mapping.
    /// </summary>
    public class LinterProxy : ILinter
    {
        /// <summary>
        /// Gets the target linter.
        /// </summary>
        public ILinter Target { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="LinterProxy"/> class.
        /// </summary>
        /// <param name="linter">The linter.</param>
        public LinterProxy(ILinter linter)
        {
            Target = linter;
        }

        /// <summary>
        /// Map <see cref="ILinterResult"/> into <see cref="ILinterModel"/>.
        /// </summary>
        /// <param name="result">The raw object model of linter results.</param>
        /// <returns>The unified object model of linter results.</returns>
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

        /// <summary>
        /// Parse <see cref="Stream"/> and <see cref="ILinterArgs"/> into <see cref="ILinterResult"/>.
        /// </summary>
        /// <param name="stream">The input stream with raw output from linter.</param>
        /// <param name="args">The original arguments which was used to execute linter.</param>
        /// <returns>The raw object model of linter results.</returns>
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