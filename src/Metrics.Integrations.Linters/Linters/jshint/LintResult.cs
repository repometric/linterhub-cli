namespace Metrics.Integrations.Linters.jshint
{
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.Serialization;

    [XmlRoot(ElementName="checkstyle")]
    public class LintResult : ILinterResult
    {
        [XmlElement("file")]
        public List<File> FilesList { get; set; }
    }
}
