namespace Linterhub.Engine
{
    using System.IO;

    /// <summary>
    /// Represents basic interface for linter.
    /// </summary>
    public interface ILinter
    {
        /// <summary>
        /// Parse <see cref="Stream"/> and <see cref="ILinterArgs"/> into <see cref="ILinterResult"/>.
        /// </summary>
        /// <param name="stream">The input stream with raw output from linter.</param>
        /// <param name="args">The original arguments which was used to execute linter.</param>
        /// <returns>The raw object model of linter results.</returns>
        ILinterResult Parse(Stream stream, ILinterArgs args);

        /// <summary>
        /// Map <see cref="ILinterResult"/> into <see cref="ILinterModel"/>.
        /// </summary>
        /// <param name="result">The raw object model of linter results.</param>
        /// <returns>The unified object model of linter results.</returns>
        ILinterModel Map(ILinterResult result);
    }
}
