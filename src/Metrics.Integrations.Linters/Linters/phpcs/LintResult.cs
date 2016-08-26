namespace Metrics.Integrations.Linters.Phpcs
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class LintResult : ILinterResult
    {
        [JsonProperty("totals")]
        public Totals Total { get; set; }
        [JsonProperty("files")]
        public Dictionary<string, File> Files {get; set;} 
    }
}