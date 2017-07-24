namespace Linterhub.Core.Schema
{
    /// <summary>
    /// Represents engine specification.
    /// </summary>
    public class EngineSpecification
    {
        /// <summary>
        /// Gets the engine schema.
        /// </summary>
        public EngineSchema Schema { get; }

        /// <summary>
        /// Gets the engine options schema.
        /// </summary>
        public EngineOptionsSchema OptionsSchema { get; }

        /// <summary>
        /// Initializes a new instance of <seealso cref="EngineSpecification"/> class.
        /// </summary>
        /// <param name="schema">The engine schema.</param>
        /// <param name="optionsSchema">The engine options schema.</param>
        public EngineSpecification(EngineSchema schema, EngineOptionsSchema optionsSchema)
        {
            Schema = schema;
            OptionsSchema = optionsSchema;
        }
    }
}