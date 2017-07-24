namespace Linterhub.Core.Runtime
{
    using System.IO;
    using Exceptions;
    using Schema;
    using System.Collections.Generic;
    using System.Linq;
    using Extensions;

    /// <summary>
    /// The engine wrapper.
    /// </summary>
    public class EngineWrapper
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
        /// Initializes a new instance of the <seealso cref="EngineWrapper"/>.
        /// </summary>
        /// <param name="terminal">The terminal wrapper.</param>
        /// <param name="commandFactory">The command factory.</param>
        public EngineWrapper(TerminalWrapper terminal, CommandFactory commandFactory)
        {
            Terminal = terminal;
            CommandFactory = commandFactory;
        }

        /// <summary>
        /// Run analysis command.
        /// </summary>
        /// <param name="context">The engine context.</param>
        /// <returns>The result.</returns>
        public string RunAnalysis(EngineWrapper.Context context, string stdin = "")
        {
            var tempFile = "#tempfile";

            if (context.Stdin == Context.stdinType.Use)
            {
                tempFile = Path.GetTempFileName();
                File.WriteAllText(tempFile, stdin);
                context.RunOptions.Remove("{path}");
                context.RunOptions.Add("{path}", Path.GetFileName(tempFile));
                context.WorkingDirectory = Path.GetDirectoryName(tempFile);
            }

            var command = CommandFactory.GetAnalyzeCommand(context);

            var result = Run(context, command, successCode: context.Specification.Schema.SuccessCode ?? 0, stdin: context.Stdin == Context.stdinType.UseWithEngine ? stdin : string.Empty)
                    .DeserializeAsJson<EngineOutputSchema.ResultType[]>()
                    .Select((file) => {
                        var dic = context.RunOptions.Where(x => x.Key == "file://{stdin}");
                        if (dic.Count() != 0)
                        {
                            file.Path = dic.First().Value;
                        }
                        return file;
                    });

            if (File.Exists(tempFile))
                File.Delete(tempFile);

            return result.SerializeAsJson();
        }

        /// <summary>
        /// Run version command.
        /// </summary>
        /// <param name="context">The engine context.</param>
        /// <returns>The result.</returns>
        public string RunVersion(EngineWrapper.Context context)
        {
            var command = CommandFactory.GetVersionCommand(context.Specification);
            return Run(context, command);
        }

        /// <summary>
        /// Run engine command.
        /// </summary>
        /// <param name="context">The engine context.</param>
        /// <param name="command">The command.</param>
        /// <param name="commandSeparator">The command separator.</param>
        /// <param name="successCode">The expected success code.</param>
        /// <returns>The result.</returns>
        protected string Run(EngineWrapper.Context context, string command, string commandSeparator = " ", int successCode = 0, string stdin = "")
        {
            var result = Terminal.RunTerminal(command, Path.GetFullPath(context.WorkingDirectory), stdin: stdin);

            if (result.RunException != null)
            {
                throw new EngineException("Running engine exception", result.RunException.Message);
            }

            var stdError = result.Error.ToString().Trim();
            var stdOut = result.Output.ToString().Trim();

            if (!string.IsNullOrEmpty(stdError) && result.ExitCode != successCode)
            {
                throw new EngineException(stdError);
            }

            if (string.IsNullOrEmpty(stdOut) && result.ExitCode != successCode)
            {
                throw new EngineException($"Unexpected exit code: {result.ExitCode}");
            }

            return string.IsNullOrEmpty(stdOut) ? stdError : stdOut;
        }

        /// <summary>
        /// The engine context.
        /// </summary>
        public class Context
        {
            /// <summary>
            /// Gets or sets the engine specification.
            /// </summary>
            public EngineSpecification Specification { get; set; }

            /// <summary>
            /// Gets or sets run options.
            /// </summary>
            public EngineOptions RunOptions { get; set; }

            /// <summary>
            /// Gets or sets configuration options.
            /// </summary>
            public EngineOptions ConfigOptions { get; set; }

            /// <summary>
            /// Gets or sets the working directory.
            /// </summary>
            public string WorkingDirectory { get; set; }

            public stdinType Stdin { get; set; } = 0;

            public enum stdinType
            {
                NotUse,
                Use,
                UseWithEngine
            }
        }
    }
}