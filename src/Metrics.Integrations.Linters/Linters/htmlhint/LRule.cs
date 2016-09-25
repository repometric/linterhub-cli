namespace Metrics.Integrations.Linters.htmlhint
{
    using Newtonsoft.Json;

    public class LRule
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Rule description
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Link to github with description of this rule
        /// </summary>
        [JsonProperty("link")]
        public string GithubLink { get; set; } 
    }
}
