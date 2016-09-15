namespace Metrics.Integrations.Linters.PhpAssumptions
{
    using System.Xml;
    using System.Xml.Serialization;
    using System.Collections.Generic;

    public class File
    {
        [XmlAttribute("name")]
        public string FilePath { get; set; }

        [XmlElement("line")]
        public List<Line> LinesList = new List<Line>();
    }
}
