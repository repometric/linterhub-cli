namespace Linterhub.Cli.Runtime
{
    using Linterhub.Engine;

    public class LinterhubWrapper
    {
        public RunContext Context { get; }
        public LinterEngine Engine { get; }

        public LinterhubWrapper(RunContext context, LinterEngine engine)
        {
            Context = context;
            Engine = engine;
        }

        public string Run(string command)
        {
            var cmd = string.Format(Context.Configuration.TerminalCommand, command);
            var run = Engine.Run(Context.Configuration.Terminal, cmd, Context.Configuration.Linterhub);
            return run.Output.ToString();
        }

        public string Info()
        {
            var result = Run(Context.Configuration.CommandInfo);
            return result;
        }

        public string Analyze(string name, string command, string path)
        {
            var cmd = string.Format(Context.Configuration.Command, name, command, path);
            var result = Run(cmd);
            return result;
        }
    }
}