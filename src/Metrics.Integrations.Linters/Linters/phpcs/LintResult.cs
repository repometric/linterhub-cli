namespace Metrics.Integrations.Linters.Phpcs
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class LintResult : ILinterResult
    {
        /// <summary>
        /// Information about all task
        /// </summary>
        [JsonProperty("totals")]
        public Totals Total { get; set; }
        [JsonProperty("files")]
        public Dictionary<string, File> Files {get; set;} 
    }
}