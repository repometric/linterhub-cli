namespace Linterhub.Core.Schema
{
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// Schema of npm package config (care, implemented only required properties)
    /// </summary>
    public class NpmPackageSchema
    {
        /// <summary>
        /// Gets or sets list of project dependencies
        /// </summary>
        public Dictionary<string, string> Dependencies = new Dictionary<string, string>();

        /// <summary>
        /// Gets or sets list of developer project dependencies
        /// </summary>
        public Dictionary<string, string> DevDependencies = new Dictionary<string, string>();
    }
}