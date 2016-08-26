using System;
using Metrics.Integrations.Linters;
using Newtonsoft.Json;

namespace Metrics.Integrations.Linters.Phpcs
{
    public class Error
    {
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("source")]
        public string Source { get; set; }
        [JsonProperty("severity")]
        public int Severity { get; set; }
        [JsonProperty("type")] 
        public string Type { get; set; }
        [JsonProperty("line")]
        public int Line { get; set; } 
        [JsonProperty("column")]
        public int Column { get; set; }
        [JsonProperty("fixable")]
        public bool Fixable { get; set; } 
    }
}
