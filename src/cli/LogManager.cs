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

        public void Log(string format, params object[] args)
        {
            LogWriter.AppendLine(string.Format(format, args));
        }

        public void Trace(string message, params object[] args)
        {
            Log("TRACE: " + message + " {0}", string.Join(" ", args));
        }

        public void Error(string message, params object[] args)
        {
            error = true;
            Log("ERROR: " + message + " {0}", string.Join(" ", args));
            System.Console.Error.WriteLine(string.Format(message + " {0}", string.Join(" ", args)));
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