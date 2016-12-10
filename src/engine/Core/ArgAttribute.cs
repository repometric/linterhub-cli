namespace Linterhub.Engine
{
    using System;

    /// <summary>
    /// Represents configuration for linter argument.
    /// </summary>
    public class ArgAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of command (prefix).
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the separator of command.
        /// </summary>
        public string Separator { get; }

        /// <summary>
        /// Gets the value indicating whether to add separator and value.
        /// </summary>
        public bool Add { get; }

        /// <summary>
        /// Gets the order of the argument.
        /// </summary>
        public int Order { get; }

        public bool Path { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ArgAttribute"/> class.
        /// </summary>
        /// <param name="name">The argument name.</param>
        /// <param name="add">Whether to add a prefix and the value.</param>
        /// <param name="separator">The separator.</param>
        /// <param name="order">The order.</param>
        /// <param name="path"></param>
        public ArgAttribute(string name = null, bool add = true, string separator = " ", int order = default(int), bool path = false)
        {
            Name = name;
            Add = add;
            Separator = separator;
            Order = order;
            Path = path;
        }
    }
}
