using System.Collections.Generic;

namespace Linterhub.Cli.Runtime
{
    using Newtonsoft.Json;

    public class ExtConfig
    {
        [JsonProperty("mode")]
        public string Mode { get; set; }

        [JsonProperty("linters")]
        public List<ExtLint> Linters { get; set; }

        public ExtConfig()
        {
            Linters = new List<ExtLint>();
        }

        public class ExtLint
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("config")]
            public object Config { get; set; }

            [JsonProperty("command")]
            public string Command { get; set; }
        }
    }
}