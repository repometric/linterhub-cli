namespace Linterhub.Cli.Runtime
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class ProjectConfig
    {
        [JsonProperty("mode")]
        public string Mode { get; set; }

        [JsonProperty("linters")]
        public List<Linter> Linters { get; set; }

        public ProjectConfig()
        {
            Linters = new List<Linter>();

        }

        public class Linter
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("config")]
            public object Config { get; set; }

            [JsonProperty("command")]
            public string Command { get; set; }

            [JsonProperty("active")]
            public bool? Active { get; set; }
        }
    }
}