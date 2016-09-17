namespace Metrics.Integrations.Linters.csslint
{
    using Newtonsoft.Json;

    public class Rule
    {
        [JsonProperty("id")]
        public string Id;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("desc")]
        public string Description;

        [JsonProperty("url")]
        public string GithubUrl;

        [JsonProperty("browsers")]
        public string Browsers = null;
    }
}