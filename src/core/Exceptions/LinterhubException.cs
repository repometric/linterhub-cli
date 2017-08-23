namespace Linterhub.Core.Exceptions
{
    using System;
    using Schema;
    using Utils;

    /// <summary>
    /// Represents basic linterhub exception.
    /// </summary>
    public class LinterhubException: Exception
    {

        public ErrorCode exitCode { get; }

        public string Title { get; }
        public enum ErrorCode
        {
            noError,
            linterhubConfig,
            missngParams,
            engineCrashed,
            engineMissing,
            pathMissing
        }

        /// <summary>
        /// Initializes a new instance of the <seealso cref="LinterhubException"/>.
        /// </summary>
        /// <param name="title">The error title.</param>
        /// <param name="description">The error description.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="statusCode">The status code.</param>
        /// </summary>
        public LinterhubException(string title, string description, ErrorCode statusCode, Exception innerException = null)
            : base(convertMessage(title, description, statusCode), innerException)
        {
            Title = title;
            exitCode = statusCode;
        }

        private static string convertMessage(string title, string message, ErrorCode statusCode)
        {
            return new LinterhubErrorSchema()
            {
                Code = (int)statusCode,
                Title = title,
                Description = message
            }.SerializeAsJson();
        }
    }
}