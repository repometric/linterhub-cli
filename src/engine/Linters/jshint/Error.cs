namespace Linterhub.Engine.Linters.jshint
{
    using System.Xml;
    using System.Xml.Serialization;

    public class Error
    {
        [XmlAttribute("line")]
        public string Line { get; set; }

        [XmlAttribute("column")]
        public string Column { get; set; }

        /// <summary>
        ///  Small description of the error
        /// </summary>
        [XmlAttribute("message")]
        public string Message { get; set; }

        [XmlAttribute("source")]
        public string Source { get; set; }

        /// <summary>
        /// Warning, error ...
        /// </summary>
        [XmlAttribute("severity")]
        public string Severity { get; set; }

    }
}
