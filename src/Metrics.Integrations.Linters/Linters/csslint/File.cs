namespace Metrics.Integrations.Linters.csslint
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class File
    {
        [JsonProperty("filename")]
        public string FilePath { get; set; }

        [JsonProperty("messages")]
        public List<Message> MessagesList { get; set; }
    }
}