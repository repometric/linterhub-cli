namespace Metrics.Integrations.Linters.htmlvnu
{
    using Newtonsoft.Json;

    public class Message
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("firstline")]
        public int FirstLine { get; set; }

        [JsonProperty("lastline")]
        public int LastLine { get; set; }

        [JsonProperty("lastcolumn")]
        public int LastColumn { get; set; }

        [JsonProperty("firstcolumn")]
        public int FirstColumn { get; set; }

        [JsonProperty("message")]
        public string Msg { get; set; }

        [JsonProperty("extract")]
        public string Extract { get; set; }

        [JsonProperty("hilitestart")]
        public int HiliteStart { get; set; }

        [JsonProperty("hilitelength")]
        public int HiliteLength { get; set; }
    }
}
