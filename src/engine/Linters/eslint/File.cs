namespace Linterhub.Engine.Linters.eslint
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class File
    {
        [JsonProperty("filePath")]
        public string FilePath { get; set; }

        [JsonProperty("messages")]
        public List<Message> Messages { get; set; }

        [JsonProperty("errorCount")]
        public int ErrorCount { get; set; }

        [JsonProperty("warningCount")]
        public int WarningCount { get; set; }
    }
}
