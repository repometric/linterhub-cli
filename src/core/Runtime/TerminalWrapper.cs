namespace Linterhub.Core.Runtime
{
    using System.IO;

    /// <summary>
    /// The terminal wrapper.
    /// </summary>
    public class TerminalWrapper : CmdWrapper
    {
        /// <summary>
        /// Gets the path to the terminal.
        /// </summary>
        protected string TerminalPath { get; private set; }

        /// <summary>
        /// Gets the format of terminal command.
        /// </summary>
        protected string TerminalCommand { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <seealso cref="TerminalWrapper"/>.
        /// </summary>
        /// <param name="terminalPath">The path to the terminal.</param>
        /// <param name="terminalCommand">The format of terminal command.</param>
        public TerminalWrapper(string terminalPath, string terminalCommand)
        {
            TerminalPath = terminalPath;
            TerminalCommand = terminalCommand;
        }

        /// <summary>
        /// Run terminal.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        /// <param name="workingDirectory">The working directory.</param>
        /// <param name="waitTimeout">The amount of time, in milliseconds, to wait for the associated process to exit.</param>
        /// <returns>The <seealso cref="Result"/>.</returns>
        public CmdWrapper.Result RunTerminal(string arguments, string workingDirectory = null, int waitTimeout = -1, string stdin = "")
        {
            var command = string.Format(TerminalCommand, arguments);
            var result = this.RunExecutable(TerminalPath, command, workingDirectory, waitTimeout, stdin);
            return result;
        }
    }
}