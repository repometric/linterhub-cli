namespace Linterhub.Engine.Linters.csslint
{
    using Newtonsoft.Json;

    public class Rule
    {
        /// <summary>
        ///  Rule class
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        ///  Rule name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        ///  Small description of problem in general
        /// </summary>
        [JsonProperty("desc")]
        public string Description { get; set; }

        /// <summary>
        ///  Url where u can find more about this
        /// </summary>
        [JsonProperty("url")]
        public string GithubUrl { get; set; }

        /// <summary>
        ///  Choose whick rules to use (by browser)
        /// </summary>
        [JsonProperty("browsers")]
        public string Browsers = null;
    }
}