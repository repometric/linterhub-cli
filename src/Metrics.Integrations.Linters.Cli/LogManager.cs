namespace Metrics.Integrations.Linters
{
    public class LogManager
    {
        public void Console(string format, params object[] args)
        {
            System.Console.WriteLine(format, args);
        }

        public void Trace(string message, params object[] args)
        {
            Console("TRACE: " + message + " {0}", string.Join(" ", args));
        }

        public void Error(string message, params object[] args)
        {
            Console("ERROR: " + message + " {0}", string.Join(" ", args));
        }
    }
}