namespace Metrics.Integrations.Linters.csslint
{
    using Newtonsoft.Json;

    public class Message
    {
        /// <summary>
        ///  Error, Warning etc
        /// </summary>
        [JsonProperty("type")]
        public string sSeverity { get; set; }

        public LinterFileModel.Error.SeverityType Severity
        {
            get
            {
                switch (sSeverity)
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

        [JsonProperty("line")]
        public int Line { get; set; }

        [JsonProperty("col")]
        public int Column { get; set; }

        /// <summary>
        ///  Wrong code
        /// </summary>
        [JsonProperty("evidence")]
        public string Evidence { get; set; }

        /// <summary>
        ///  Description of problem
        /// </summary>
        [JsonProperty("message")]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///  Rule details
        /// </summary>
        [JsonProperty("rule")]
        public Rule LRule { get; set; }
    }
}