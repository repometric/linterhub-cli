namespace Linterhub.Cli.Runtime
{
    using Newtonsoft.Json;

    public class ExtConfig
    {
        [JsonProperty("mode")]
        public string Mode { get; set; }

        [JsonProperty("linters")]
        public ExtLint[] Linters { get; set; }

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