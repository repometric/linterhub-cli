namespace Metrics.Integrations.Linters.phpcheckstyle
{
    using System.Xml;
    using System.Xml.Serialization;
    using System.Collections.Generic;

    public class File
    {
        [XmlAttribute("name")]
        public string FilePath { get; set; }

        [XmlElement("error")]
        public List<Error> ErrorsList = new List<Error>();
    }
}
