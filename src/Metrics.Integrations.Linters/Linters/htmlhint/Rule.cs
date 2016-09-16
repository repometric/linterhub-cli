namespace Metrics.Integrations.Linters.htmlhint
{
    using Newtonsoft.Json;

    public class Rule
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Link to github with description of this rule
        /// </summary>
        [JsonProperty("link")]
        public string GithubLink { get; set; } 
    }
}
