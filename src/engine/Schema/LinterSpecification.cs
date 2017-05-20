namespace Linterhub.Engine.Schema
{
    /// <summary>
    /// Represents linter specification.
    /// </summary>
    public class LinterSpecification
    {
        /// <summary>
        /// Gets the linter schema.
        /// </summary>
        public EngineSchema Schema { get; }

        /// <summary>
        /// Gets the linter options schema.
        /// </summary>
        public LinterOptionsSchema OptionsSchema { get; }

        /// <summary>
        /// Initializes a new instance of <seealso cref="LinterSpecification"/> class.
        /// </summary>
        /// <param name="schema">The linter schema.</param>
        /// <param name="optionsSchema">The linter options schema.</param>
        public LinterSpecification(EngineSchema schema, LinterOptionsSchema optionsSchema)
        {
            Schema = schema;
            OptionsSchema = optionsSchema;
        }
    }
}