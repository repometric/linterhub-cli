namespace Metrics.Integrations.Linters.pmd
{
    using System.Xml.Serialization;
    using System.Collections.Generic;

    [XmlRoot(ElementName = "pmd")]
    public class LintResult : ILinterResult
    {
        [XmlElement("file")]
        public List<File> Files { get; set; } 
    }
}
