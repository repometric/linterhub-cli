namespace Linterhub.Engine.Linters.Phpcs
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class File
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
        /// List of messages
        /// </summary>
        [JsonProperty("messages")]
        public List<Error> Messages { get; set; }
    }
}
