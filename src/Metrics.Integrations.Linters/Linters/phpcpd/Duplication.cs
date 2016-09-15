namespace Metrics.Integrations.Linters.Phpcpd
{
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.Serialization;

    public class Duplication
    {
        [XmlElement("file")]
        public List<File> FilesList = new List<File>();
        [XmlAttribute("lines")]
        public string Lines { get; set; }
        [XmlAttribute("tokens")]
        public string Tokens { get; set; }
        [XmlElement("codefragment")]
        public string Codefragment;
    }
}
