namespace Metrics.Integrations.Linters.Phpcpd
{
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.Serialization;

    [XmlRoot(ElementName="pmd-cpd")]
    public class LintResult : ILinterResult, ILinterModel
    {
        [XmlElement("duplication")]
        public List<Duplication> DuplicationsList { get; set; }
    }
}
