namespace Linterhub.Engine.Runtime
{
    using System.IO;
    using Linterhub.Engine.Exceptions;
    using Linterhub.Engine.Schema;

    /// <summary>
    /// The linter wrapper.
    /// </summary>
    public class LinterWrapper
    {
        /// <summary>
        /// Gets the terminal wrapper.
        /// </summary>
        protected TerminalWrapper Terminal { get; private set; }

        /// <summary>
        /// Gets the command factory.
        /// </summary>
        protected CommandFactory CommandFactory { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <seealso cref="LinterWrapper"/>.
        /// </summary>
        /// <param name="terminal">The terminal wrapper.</param>
        /// <param name="commandFactory">The command factory.</param>
        public LinterWrapper(TerminalWrapper terminal, CommandFactory commandFactory)
        {
            Terminal = terminal;
            CommandFactory = commandFactory;
        }

        /// <summary>
        /// Run analysis command.
        /// </summary>
        /// <param name="context">The linter context.</param>
        /// <returns>The result.</returns>
        public string RunAnalysis(LinterWrapper.Context context)
        {
            var command = CommandFactory.GetAnalyzeCommand(context.Specification, context.RunOptions, context.ConfigOptions);
            return Run(context, command, successCode: context.Specification.Schema.SuccessCode ?? 0);
        }

        /// <summary>
        /// Run version command.
        /// </summary>
        /// <param name="context">The linter context.</param>
        /// <returns>The result.</returns>
        public string RunVersion(LinterWrapper.Context context)
        {
            var command = CommandFactory.GetVersionCommand(context.Specification);
            return Run(context, command);
        }

        /// <summary>
        /// Run linter command.
        /// </summary>
        /// <param name="context">The linter context.</param>
        /// <param name="command">The command.</param>
        /// <param name="commandSeparator">The command separator.</param>
        /// <param name="successCode">The expected success code.</param>
        /// <returns>The result.</returns>
        protected string Run(LinterWrapper.Context context, string command, string commandSeparator = " ", int successCode = 0)
        {
            var result = Terminal.RunTerminal(command, Path.GetFullPath(context.WorkingDirectory));

            if (result.RunException != null)
            {
                throw new LinterEngineException(result.RunException);
            }

            var stdError = result.Error.ToString().Trim();
            var stdOut = result.Output.ToString().Trim();

            if (!string.IsNullOrEmpty(stdError) && result.ExitCode != successCode)
            {
                throw new LinterEngineException(stdError);
            }

            if (string.IsNullOrEmpty(stdOut) && result.ExitCode != successCode)
            {
                throw new LinterEngineException($"Unexpected exit code: {result.ExitCode}");
            }

            return string.IsNullOrEmpty(stdOut) ? stdError : stdOut;
        }

        /// <summary>
        /// The linter context.
        /// </summary>
        public class Context
        {
            /// <summary>
            /// Gets or sets the linter specification.
            /// </summary>
            public LinterSpecification Specification { get; set; }

            /// <summary>
            /// Gets or sets run options.
            /// </summary>
            public LinterOptions RunOptions { get; set; }

            /// <summary>
            /// Gets or sets configuration options.
            /// </summary>
            public LinterOptions ConfigOptions { get; set; }

            /// <summary>
            /// Gets or sets the working directory.
            /// </summary>
            public string WorkingDirectory { get; set; }
        }
    }
}