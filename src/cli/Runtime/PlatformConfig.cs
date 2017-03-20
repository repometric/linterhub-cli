namespace Linterhub.Cli.Runtime
{
    /// <summary>
    /// Represents platform configuration.
    /// </summary>
    public class PlatformConfig
    {
        /// <summary>
        /// Gets or sets platform specific commands. 
        /// </summary>
        public CommandSection Command { get; set; }

        /// <summary>
        /// Gets or sets the platform specific terminal configurations.
        /// </summary>
        public TerminalSection Terminal { get; set; }

        /// <summary>
        /// Represents platform specific commands section.
        /// </summary>
        public class CommandSection
        {
            /// <summary>
            /// Gets or sets the command indicating whether the app is installed.
            /// </summary>
            public string Installed { get; set; }
        }

        /// <summary>
        /// Represents terminal configuration section.
        /// </summary>
        public class TerminalSection
        {
            /// <summary>
            /// Gets or sets the path to the terminal.
            /// </summary>
            public string Path { get; set; }

            /// <summary>
            /// Gets or sets the format of commands for the terminal.
            /// </summary>
            public string Command { get; set; }
        }
    }
}