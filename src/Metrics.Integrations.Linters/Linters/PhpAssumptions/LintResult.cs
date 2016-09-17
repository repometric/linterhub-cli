namespace Metrics.Integrations.Linters.PhpAssumptions
{
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.Serialization;

    [XmlRoot(ElementName="phpa")]
    public class LintResult : ILinterResult
    {
        /// <summary>
        ///  The percentage of "bad" code
        /// </summary>
        [XmlAttribute("percentage")]
        public string Percentage { get; set; }

        /// <summary>
        ///  Number of bool expresions in the code
        /// </summary>
        [XmlAttribute("bool-expressions")]
        public string BoolExpressions { get; set; }

        /// <summary>
        ///  Number of assumptions
        /// </summary>
        [XmlAttribute("assumptions")]
        public string Assumptions { get; set; }

        [XmlArray("files")]
        [XmlArrayItem("file")]
        public List<File> FilesList; 
    }
}
