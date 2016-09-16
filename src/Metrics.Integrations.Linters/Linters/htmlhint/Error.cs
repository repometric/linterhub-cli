namespace Metrics.Integrations.Linters.htmlhint
{
    using Newtonsoft.Json;

    public class Error
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("evidence")]
        public string Evidence { get; set; }

        /// <summary>
        /// Type of error (error, warning, ...)
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("raw")]
        public string Raw { get; set; }

        [JsonProperty("column")]
        public int Column { get; set; }

        [JsonProperty("line")]
        public int Line { get; set; }

        [JsonProperty("rule")]
        public LRule Rule { get; set; } 
    }
}
