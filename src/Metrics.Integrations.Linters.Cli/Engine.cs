namespace Metrics.Integrations.Linters
{
    using System.IO;
    using System.Text;
    using Extensions;
    using Runtime;

    public class Engine
    {
        private LogManager log { get; }
        private LinterContext context { get; }

        public Engine(LinterContext context, LogManager log)
        {
            this.context = context;
            this.log = log;
        }

        // TODO: Add more logs.
        public ILinterModel Run(ILinter linter, ILinterArgs args)
        {
            // TODO: Think about not docker config
            var argString = new ArgBuilder().Build(args);
            var cmd = string.Format(
                context.Configuration.Command,
                context.RunContext.Linter,
                argString,
                context.OutputFileName,
                args.TestPath);
            log.Trace("Prepared command:", cmd);

            var wrapper = new CmdWrapper();
            var run = wrapper.RunExecutable(
                context.Configuration.Terminal, 
                string.Format(context.Configuration.TerminalCommand, cmd), 
                context.Configuration.Linterhub);
            log.Trace("Exec status:", run.ExitCode);

            if (run.RunException != null)
            {
                throw run.RunException;
            }

            var output = File.ReadAllText(context.OutputFullPath);
            var logFolder = context.GetLinterLogFolder();
            log.Trace("Log folder", logFolder);
            if (Directory.Exists(logFolder))
            {
                log.Trace("Write logs");
                File.WriteAllText(context.GetLinterLogFile("output"), run.Output?.ToString());
                File.WriteAllText(context.GetLinterLogFile("error"), run.Error?.ToString());
                File.WriteAllText(context.GetLinterLogFile("raw"), output);
            }

            File.Delete(context.OutputFullPath);

            // TODO: Read stream from stdout.
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(output)))
            {
                var result = linter.Parse(stream, args);
                var map = linter.Map(result);
                log.Trace("Linter output parsed");
                return map;
            }
        }
    }
}
