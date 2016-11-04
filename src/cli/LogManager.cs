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

        public void Error(string message, params object[] args)
        {
            error = true;
            Log("ERROR: " + message + " " + string.Join(" ", args));
            System.Console.Error.WriteLine(message + " " + string.Join(" ", args));
        }

        public void Error(Exception exception)
        {
            Error(exception.Message, exception.StackTrace);
        }

        public void Save(string fileName)
        {
            saved = true;
            File.WriteAllText(fileName, LogWriter.ToString());
        }

        public void Dispose()
        {
            if (!saved && error)
            {
                System.Console.Write(LogWriter.ToString());
            }
        }
    }
}