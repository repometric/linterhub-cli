namespace Metrics.Integrations.Linters.htmlhint
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class File
    {
        [JsonProperty("file")]
        public string FilePath { get; set; }

        /// <summary>
        /// Time spend for analysis (ms)
        /// </summary>
        [JsonProperty("time")]
        public int Time { get; set; }

        [JsonProperty("messages")]
        public List<Error> Messages { get; set; }
    }
}
