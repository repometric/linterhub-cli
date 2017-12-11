namespace Linterhub.Engine.Exceptions
{
    using System;

    /// <summary>
    /// Represents basic linter exception.
    /// </summary>
    public class LinterException: Exception
    {
        /// <summary>
        /// Gets the status code.
        /// </summary>
        public int StatusCode { get; } = default(int);

        /// <summary>
        /// Initializes a new instance of the <seealso cref="LinterException"/>.
        /// </summary>
        /// <param name="message">The message.</param>
        public LinterException(string message)
            :this(message, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <seealso cref="LinterException"/>.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        public LinterException(Exception innerException)
            :this(innerException.Message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <seealso cref="LinterException"/>.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param> <summary>
        /// <paramref name="statusCode">The status code.</param>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        /// <param name="statusCode"></param>
        public LinterException(string message, Exception innerException, int statusCode = default(int))
            :base(message, innerException)
        {
            StatusCode = statusCode;
        }
    }
}