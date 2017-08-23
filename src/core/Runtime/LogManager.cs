namespace Linterhub.Core.Runtime
{
    using System;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Represents simple logger.
    /// </summary>
    public class LogManager : IDisposable
    {
        /// <summary>
        /// Gets the log writer.
        /// </summary>
        public StringBuilder LogWriter { get; }

        private string lastError;
        private bool saved = false;
        private bool error = false;

        /// <summary>
        /// Initializes a new instance of the <seealso cref="LogManager"/>.
        /// </summary>
        public LogManager()
        {
            LogWriter = new StringBuilder();
        }

        /// <summary>
        /// Log message.
        /// </summary>
        /// <param name="message">The message content.</param>
        public void Log(string message)
        {
            LogWriter.AppendLine(message);
        }

        /// <summary>
        /// Log error.
        /// </summary>
        /// <param name="message">The message content.</param>
        public void Error(string message)
        {
            error = true;
            lastError = message;
            Log(message);
        }

        /// <summary>
        /// Log error.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public void Error(Exception exception)
        {
            Error(exception.Message);
        }

        /// <summary>
        /// Save log content.
        /// </summary>
        /// <param name="fileName">The target file, if null will output the last error to console.</param>
        public void Save(string fileName = null)
        {
            saved = true;
            if (!string.IsNullOrEmpty(fileName))
            {
                File.WriteAllText(fileName, LogWriter.ToString());
            }
            else if (!string.IsNullOrEmpty(lastError))
            {
                Console.WriteLine(lastError);
            }
        }

        /// <summary>
        /// Dispose logger instance.
        /// </summary>
        public void Dispose()
        {
            if (!saved && error)
            {
                Save();
            }
        }
    }
}