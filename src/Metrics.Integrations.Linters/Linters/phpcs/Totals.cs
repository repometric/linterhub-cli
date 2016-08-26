using System;
using Metrics.Integrations.Linters;
using Newtonsoft.Json;

namespace Metrics.Integrations.Linters.Phpcs
{
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
