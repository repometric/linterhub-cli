namespace Metrics.Integrations.Linters.csslint
{
    using Newtonsoft.Json;

    public class Message
    {
        [JsonProperty("type")]
        public string Severity;

        [JsonProperty("line")]
        public int Line;

        [JsonProperty("col")]
        public int Column;

        [JsonProperty("evidence")]
        public string Evidence;

        [JsonProperty("message")]
        public string ErrorMessage;

        [JsonProperty("rule")]
        public Rule LRule;
    }
}