using System;
using System.Collections.Generic;
using Metrics.Integrations.Linters;
using Newtonsoft.Json;

namespace Metrics.Integrations.Linters.Phpcs
{
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
