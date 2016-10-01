namespace Metrics.Integrations.Linters.Phpmd
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    public class File
    {
        [XmlElement("violation")]
        public List<Violation> ViolationsList { get; set; }
        [XmlAttribute("name")]
        public string FileName { get; set; }
    }
}
