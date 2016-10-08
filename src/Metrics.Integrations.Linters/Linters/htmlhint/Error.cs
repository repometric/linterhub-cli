namespace Metrics.Integrations.Linters.htmlhint
{
    using Newtonsoft.Json;

    public class Error
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        /// Changed code
        /// </summary>
        [JsonProperty("evidence")]
        public string Evidence { get; set; }

        /// <summary>
        /// Type of error (error, warning, ...)
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        public LinterFileModel.Error.SeverityType Severity
        {
            get
            {
                switch (Type)
                {
                    case "warning": return LinterFileModel.Error.SeverityType.warning;
                    case "error": return LinterFileModel.Error.SeverityType.error;
                    default: return LinterFileModel.Error.SeverityType.warning;
                }
            }
            set
            {
                Severity = value;
            }
        }

        /// <summary>
        /// Wrong code
        /// </summary>
        [JsonProperty("raw")]
        public string Raw { get; set; }

        [JsonProperty("column")]
        public int Column { get; set; }

        [JsonProperty("line")]
        public int Line { get; set; }

        [JsonProperty("rule")]
        public LRule Rule { get; set; } 
    }
}
