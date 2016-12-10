namespace Linterhub.Cli.Runtime
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents configuration for project (.linterhub.json file).
    /// </summary>
    public class ProjectConfig
    {
        /// <summary>
        /// Gets or sets the CLI run mode.
        /// </summary>
        [JsonProperty("mode")]
        public string Mode { get; set; }

        /// <summary>
        /// Gets or sets the list of linters for the project.
        /// </summary>
        [JsonProperty("linters")]
        public List<Linter> Linters { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ProjectConfig"/> class.
        /// </summary>
        public ProjectConfig()
        {
            Linters = new List<Linter>();

        }

        /// <summary>
        /// Represents a Linter in a project config.
        /// </summary>
        public class Linter
        {
            /// <summary>
            /// Gets or sets the linter name.
            /// </summary>
            [JsonProperty("name")]
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets the config object. See also <see cref="Engine.ILinterArgs"/>.
            /// </summary>
            [JsonProperty("config")]
            public object Config { get; set; }

            /// <summary>
            /// Gets or sets the default command for linter.
            /// </summary>
            [JsonProperty("command")]
            public string Command { get; set; }

            /// <summary>
            /// Gets or sets the value indicating whether linter is active.
            /// </summary>
            [JsonProperty("active")]
            public bool? Active { get; set; }
        }
    }
}