namespace Metrics.Integrations.Linters.csslint
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class File
    {
        [JsonProperty("filename")]
        public string FilePath;

        [JsonProperty("messages")]
        public List<Message> MessagesList;
    }
}