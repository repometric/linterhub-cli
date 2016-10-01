namespace Metrics.Integrations.Linters.csslint
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class File
    {
        /// <summary>
        /// Path of tested file
        /// </summary>
        [JsonProperty("filename")]
        public string FilePath { get; set; }

        /// <summary>
        /// List of Messages (Errors) in file
        /// </summary>
        [JsonProperty("messages")]
        public List<Message> MessagesList { get; set; }
    }
}