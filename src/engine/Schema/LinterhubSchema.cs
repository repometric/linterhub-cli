namespace Linterhub.Engine.Schema
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents configuration for project (.linterhub file).
    /// </summary>
    public class LinterhubSchema
    {
        /// <summary>
        /// Gets or sets the CLI run mode.
        /// </summary>
        public string Mode { get; set; }

        /// <summary>
        /// Gets or sets the list of linters for the project.
        /// </summary>
        public List<Linter> Linters { get; set; }

        /// <summary>
        /// Gets or sets ignore rules for the whole project.
        /// </summary>
        public List<IgnoreRule> Ignore { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="Linterhub"/> class.
        /// </summary>
        public LinterhubSchema()
        {
            Linters = new List<Linter>();
            Ignore = new List<IgnoreRule>();
        }

        public class IgnoreRule
        {
            /// <summary>
            /// Gets or sets file to ignore.
            /// </summary>
            public string Path { get; set; }

            /// <summary>
            /// Gets or sets line with error to ignore.
            /// </summary>
            public int? Line { get; set; }

            /// <summary>
            /// Gets or sets error to ignore.
            /// </summary>
            public string RuleId { get; set; }
        }

        /// <summary>
        /// Represents a Linter in a project config.
        /// </summary>
        public class Linter
        {
            /// <summary>
            /// Gets or sets the linter name.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets the config object.
            /// </summary>
            public LinterOptions Config { get; set; }

            /// <summary>
            /// Gets or sets the default command for linter.
            /// </summary>
            public string Command { get; set; }

            /// <summary>
            /// Gets or sets the value indicating whether linter is active.
            /// </summary>
            public bool? Active { get; set; }

            /// <summary>
            /// Gets or sets ignore rules for linter.
            /// </summary>
            public List<IgnoreRule> Ignore { get; set; }

            /// <summary>
            /// Initializes a new instance of <see cref="Linter"/> class.
            /// </summary> 
            public Linter()
            {
                Ignore = new List<IgnoreRule>();
            }
        }
    }
}
