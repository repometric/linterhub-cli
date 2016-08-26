namespace Metrics.Integrations.Linters.Phpcs
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class File
    {
        [JsonProperty("errors")]
        public int Errors;
        [JsonProperty("warnings")]
        public int Warnings;
        [JsonProperty("messages")]
        public List<Error> Messages { get; set; }
    }
}
