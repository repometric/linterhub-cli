namespace Metrics.Integrations.Linters.phpcheckstyle
{
    using System.Xml;
    using System.Xml.Serialization;

    public class Error
    {
        [XmlAttribute("line")]
        public string Line { get; set; }

        [XmlAttribute("column")]
        public string Column { get; set; }

        [XmlAttribute("message")]
        public string Message { get; set; }

        [XmlAttribute("source")]
        public string Source { get; set; }

        /// <summary>
        /// Warning, error ...
        /// </summary>
        [XmlAttribute("severity")]
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
    }
}
