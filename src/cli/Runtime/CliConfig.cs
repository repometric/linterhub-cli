namespace Linterhub.Cli.Runtime
{
    using Newtonsoft.Json;

    public class CliConfig
    {
        [JsonProperty("command")]
        public CommandSection Command { get; set; }
        [JsonProperty("linterhub")]
        public LinterhubSection Linterhub { get; set; }
        [JsonProperty("terminal")]
        public TerminalSection Terminal { get; set; }

        public class CommandSection
        {
            [JsonProperty("analyze")]
            public string Analyze { get; set; }
            [JsonProperty("info")]
            public string Info { get; set; }
            [JsonProperty("version")]
            public string Version { get; set; }
            [JsonProperty("linterVersion")]
            public string LinterVersion { get; set; }
        }

        public class TerminalSection
        {
            [JsonProperty("path")]
            public string Path { get; set; }
            [JsonProperty("command")]
            public string Command { get; set; }
        }

        public class LinterhubSection
        {
            [JsonProperty("path")]
            public string Path { get; set; }
            [JsonProperty("command")]
            public string Command { get; set; }
        }
    }
}