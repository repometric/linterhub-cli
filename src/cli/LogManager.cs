namespace Linterhub.Cli
{
    using System;
    using System.IO;
    using System.Text;

    // TODO: Improve logging.
    public class LogManager : IDisposable
    {
        public StringBuilder LogWriter { get; }
        private bool saved = false;
        private bool error = false;

        public LogManager()
        {
            LogWriter = new StringBuilder();
        }

        public void Log(string format)
        {
            LogWriter.AppendLine(format);
        }

        public void Trace(string message, params object[] args)
        {
            Log("TRACE: " + message + " " + string.Join(" ", args));
        }

        public void Error(string message)
        {
            error = true;
            Log("ERROR: " + message);
            Console.Error.WriteLine(message);
        }

        public void Error(Exception exception)
        {
            Error(exception.Message);
            Error(exception.ToString());
        }

        public void Save(string fileName = null)
        {
            saved = true;
            if (string.IsNullOrEmpty(fileName))
            {
                Console.Write(LogWriter.ToString());
            }
            else
            {
                File.WriteAllText(fileName, LogWriter.ToString());
            }
        }

        public void Dispose()
        {
            if (!saved && error)
            {
                Save();
            }
        }
    }
}