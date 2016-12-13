namespace Linterhub.Engine.Linters.prospector
{
    using Newtonsoft.Json;

    public class Message
    {
        [JsonProperty("code")]
        public string WarningCode { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("location")]
        public Location WarningLocation { get; set; }

        /// <summary>
        ///  Description of problem
        /// </summary>
        [JsonProperty("message")]
        public string Description { get; set; }

    }
}