namespace Metrics.Integrations.Linters.csslint
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class File
    {
        [JsonProperty("filename")]
        /// <summary>
        ///  Path of tested file
        /// </summary>
        public string FilePath { get; set; }

        [JsonProperty("messages")]
        /// <summary>
        ///  List of Messages (Errors) in file
        /// </summary>
        public List<Message> MessagesList { get; set; }
    }
}