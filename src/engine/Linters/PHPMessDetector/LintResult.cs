namespace Linterhub.Engine.Linters.Phpmd
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlRoot(ElementName="pmd")]
    public class LintResult : ILinterResult
    {
        [XmlElement("file")]
        public List<File> FilesList = new List<File>(); 
    }
}
