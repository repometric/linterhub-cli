namespace Metrics.Integrations.Linters.csslint
{
    using Newtonsoft.Json;

    public class Rule
    {
        /// <summary>
        ///  Rule class
        /// </summary>
        [JsonProperty("id")]
        public string Id;

        /// <summary>
        ///  Rule name
        /// </summary>
        [JsonProperty("name")]
        public string Name;

        /// <summary>
        ///  Small description of problem in general
        /// </summary>
        [JsonProperty("desc")]
        public string Description;

        /// <summary>
        ///  Url where u can find more about this
        /// </summary>
        [JsonProperty("url")]
        public string GithubUrl;

        [JsonProperty("browsers")]
        public string Browsers = null;
    }
}