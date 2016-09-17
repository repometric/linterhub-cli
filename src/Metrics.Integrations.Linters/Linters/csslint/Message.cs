namespace Metrics.Integrations.Linters.csslint
{
    using Newtonsoft.Json;

    public class Message
    {
        /// <summary>
        ///  Error, Warning etc
        /// </summary>
        [JsonProperty("type")]
        public string Severity;

        [JsonProperty("line")]
        public int Line;

        [JsonProperty("col")]
        public int Column;

        /// <summary>
        ///  Wrong code
        /// </summary>
        [JsonProperty("evidence")]
        public string Evidence;

        /// <summary>
        ///  Description of problem
        /// </summary>
        [JsonProperty("message")]
        public string ErrorMessage;

        /// <summary>
        ///  Rule details
        /// </summary>
        [JsonProperty("rule")]
        public Rule LRule;
    }
}