namespace Linterhub.Engine.Linters.prospector
{
    using Newtonsoft.Json;

    public class Location
    {
        [JsonProperty("character")]
        public int? Column { get; set; }

        [JsonProperty("module")]
        public string Module { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("line")]
        public int? Line { get; set; }

        [JsonProperty("function")]
        public string Function { get; set; }

    }
}