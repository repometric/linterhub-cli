namespace Linterhub.Engine.Linters.Phpcpd
{
    using System.Xml;
    using System.Xml.Serialization;

    public class File
    {
        [XmlAttribute("path")]
        public string FilePath { get; set; }

        [XmlAttribute("line")]
        public string StartLine { get; set; }
    }
}
