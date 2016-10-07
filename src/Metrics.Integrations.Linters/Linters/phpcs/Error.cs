namespace Metrics.Integrations.Linters.Phpcs
{
    using Newtonsoft.Json;

    public class Error
    {
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("source")]
        public string Source { get; set; }
        [JsonProperty("severity")]
        public int Severity { get; set; }
        /// <summary>
        /// Type of error (WARNING or ERROR)
        /// </summary>
        [JsonProperty("type")]
        public string sType { get; set; }

        public LinterFileModel.Error.SeverityType lfmSeverity
        {
            get
            {
                switch (sType)
                {
                    case "WARNING": return LinterFileModel.Error.SeverityType.warning;
                    case "ERROR": return LinterFileModel.Error.SeverityType.error;
                    default: return LinterFileModel.Error.SeverityType.warning;
                }
            }
            set
            {
                lfmSeverity = value;
            }
        }

        [JsonProperty("line")]
        public int Line { get; set; } 
        [JsonProperty("column")]
        public int Column { get; set; }
        /// <summary>
        /// Can phpcs fix this automatically or not
        /// </summary>
        [JsonProperty("fixable")]
        public bool Fixable { get; set; } 
    }
}
