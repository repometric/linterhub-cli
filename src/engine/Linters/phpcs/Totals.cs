namespace Linterhub.Engine.Linters.Phpcs
{
    using Newtonsoft.Json;

    public class Totals
    {
        /// <summary>
        /// Number of errors
        /// </summary>
        [JsonProperty("errors")]
        public int Errors { get; set; }

        /// <summary>
        /// Number of warnings
        /// </summary>
        [JsonProperty("warnings")]
        public int Warnings { get; set; }

        /// <summary>
        /// Number of fixable problems
        /// </summary>
        [JsonProperty("fixable")]
        public int Fixable { get; set; } 
    }
}
