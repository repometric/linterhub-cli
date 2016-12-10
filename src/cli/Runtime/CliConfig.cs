namespace Linterhub.Cli.Runtime
{
    using Newtonsoft.Json;

    public class CliConfig
    {
        [JsonProperty("command")]
        public string Command { get; set; }
        [JsonProperty("command_info")]
        public string CommandInfo { get; set; }
        [JsonProperty("linterhub")]
        public string Linterhub { get; set; }
        [JsonProperty("terminal")]
        public string Terminal { get; set; }
        [JsonProperty("terminalCommand")]
        public string TerminalCommand { get; set; }
        [JsonProperty("projectConfig")]
        public string ProjectConfig { get; set; }
    }
}