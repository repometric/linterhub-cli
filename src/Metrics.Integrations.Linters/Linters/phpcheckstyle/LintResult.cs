namespace Metrics.Integrations.Linters.phpcheckstyle
{
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.Serialization;

    [XmlRoot(ElementName="checkstyle")]
    public class LintResult : ILinterResult, ILinterModel
    {
        [XmlElement("file")]
        public List<File> FilesList = new List<File>(); 
    }
}
