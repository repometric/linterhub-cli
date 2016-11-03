namespace Linterhub.Engine.Linters.eslint
{
    using Newtonsoft.Json;

    public class Message
    {
        [JsonProperty("ruleId")]
        public string RuleId { get; set; }

        [JsonProperty("severity")]
        public int Severity { get; set; }

        [JsonProperty("message")]
        public string MessageError { get; set; }

        [JsonProperty("line")]
        public int Line { get; set; }

        [JsonProperty("column")]
        public int Column { get; set; }

        [JsonProperty("nodeType")]
        public string NodeType { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }
    }
}
