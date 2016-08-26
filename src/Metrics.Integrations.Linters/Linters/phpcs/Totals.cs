namespace Metrics.Integrations.Linters.Phpcs
{
    using Newtonsoft.Json;

    public class Totals
    {
        [JsonProperty("errors")]
        public int Errors { get; set; }
        [JsonProperty("warnings")]
        public int Warnings { get; set; } 
        [JsonProperty("fixable")]
        public int Fixable { get; set; } 
    }
}
