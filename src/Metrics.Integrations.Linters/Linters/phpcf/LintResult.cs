namespace Metrics.Integrations.Linters.Phpcf
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlRoot(ElementName="pmd-cpd")]
    public class LintResult : ILinterResult, ILinterModel
    {
        public List<Warning> WarningsList = new List<Warning>(); 
    }
}
