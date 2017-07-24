namespace Linterhub.Core.Schema
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents definition of engine options where key is the option name.
    /// </summary>
    public class EngineOptionsSchema: Dictionary<string, EngineOptionsSchema.Item>
    {
        /// <summary>
        /// Option definition.
        /// </summary>
        public class Item
        {
            /// <summary>
            /// Gets or sets the id.
            /// </summary>
            public string Id { get; set; }

            /// <summary>
            /// Gets or sets the type.
            /// </summary>
            public string Type { get; set; }

            /// <summary>
            /// Gets or sets the description.
            /// </summary>
            /// <returns></returns>
            public string Description { get; set; }
        }
    }
}