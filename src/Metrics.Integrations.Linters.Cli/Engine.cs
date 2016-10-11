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

        public CmdWrapper.RunResults Run(ILinter linter, ILinterArgs args)
        {
            // TODO: Think about not docker config
            var argString = new ArgBuilder().Build(args);
            var cmd = string.Format(
                context.Configuration.Command,
                context.RunContext.Linter,
                argString,
                context.OutputFileName,
                args.TestPath);
            // TODO: Move logging one level upper.
            log.Trace("Prepared command:", cmd);

            var wrapper = new CmdWrapper();
            var run = wrapper.RunExecutable(
                context.Configuration.Terminal, 
                string.Format(context.Configuration.TerminalCommand, cmd), 
                context.Configuration.Linterhub);

            return run;
        }

        public ILinterResult Parse(ILinter linter, ILinterArgs args, string output)
        {
            // TODO: Read stream from stdout.
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(output)))
            {
                var result = linter.Parse(stream, args);
                return result;
            }
        }

        public ILinterModel Map(ILinter linter, ILinterResult result)
        {
            return linter.Map(result);
        }
    }
}
