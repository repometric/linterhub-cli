namespace Linterhub.Engine.Linters.Phpcpd
{
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.Serialization;

    public class Duplication
    {
        /// <summary>
        /// Contains List of Files where this duplication was found
        /// </summary>
        [XmlElement("file")]
        public List<File> FilesList { get; set; }

        /// <summary>
        /// Number of lines
        /// </summary>
        [XmlAttribute("lines")]
        public string Lines { get; set; }

        /// <summary>
        /// Number of tokens
        /// </summary>
        [XmlAttribute("tokens")]
        public string Tokens { get; set; }

        /// <summary>
        /// Duplicated code fragment
        /// </summary>
        [XmlElement("codefragment")]
        public string Codefragment { get; set; }
    }
}
