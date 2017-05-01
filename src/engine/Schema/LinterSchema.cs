namespace Linterhub.Engine.Schema
{
    /// <summary>
    /// Represents linter schema.
    /// </summary>
    public class LinterSchema
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        public VersionDefinition Version { get; set; }

        /// <summary>
        /// Gets or sets the url.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the supported languages.
        /// </summary>
        public string[] Languages { get; set; }

        /// <summary>
        /// Gets or sets the license.
        /// </summary>
        public string License { get; set; }

        /// <summary>
        /// Gets or sets the default options.
        /// </summary>
        /// <returns></returns>
        public LinterOptions Defaults { get; set; }

        /// <summary>
        /// Gets or sets the areas.
        /// </summary>
        public string[] Areas { get; set; }

        /// <summary>
        /// Gets or sets the extensions.
        /// </summary>
        public string[] Extensions { get; set; }

        /// <summary>
        /// Gets or sets the requirements.
        /// </summary>
        public RequirementDefinition[] Requirements { get; set; }

        /// <summary>
        /// Gets or sets the success exit code.
        /// </summary>
        public int SuccessCode { get; set; }

        /// <summary>
        /// Represents the version definition.
        /// </summary>
        public class VersionDefinition
        {
            /// <summary>
            /// Gets or sets the package version.
            /// </summary>
            public string Package { get; set; }

            /// <summary>
            /// Gets or sets the local version.
            /// </summary>
            public string Local { get; set; }
        }

        /// <summary>
        /// Represents the requirement definition.
        /// </summary>
        public class RequirementDefinition
        {
            /// <summary>
            /// Gets or sets the manager name.
            /// </summary>
            public string Manager { get; set; }

            /// <summary>
            /// Gets or sets the package name.
            /// </summary>
            public string Package { get; set; }
        }

        /// <summary>
        /// Gets or sets the postfix command.
        /// </summary>
        public string Postfix { get; set; }

        /// <summary>
        /// Gets or sets the preifx command.
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether linter is active (special filter for Catalog).
        /// </summary>
        public bool? Active { get; set; }

        /// <summary>
        /// Delimiter for options (space by default)
        /// </summary>
        public string OptionsDelimiter { get; set; }
    }
}