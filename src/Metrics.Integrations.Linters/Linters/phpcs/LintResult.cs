using System;
using System.Collections.Generic;
using Metrics.Integrations.Linters;
using Newtonsoft.Json;

namespace Metrics.Integrations.Linters.Phpcs
{
    public class LintResult : ILinterResult, ILinterModel
    {
        [JsonProperty("totals")]
        public Totals Total { get; set; }
        [JsonProperty("files")]
        public Dictionary<string, File> Files {get; set;} 
    }
}