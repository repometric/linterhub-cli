namespace Linterhub.Engine.Linters.Phpsa
{
    using Newtonsoft.Json;

    public class Error
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("file")]
        public string File { get; set; }

        /// <summary>
        /// Line number
        /// </summary>
        [JsonProperty("line")]
        public int Line { get; set; }

        /// <summary>
        /// Type of error
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
