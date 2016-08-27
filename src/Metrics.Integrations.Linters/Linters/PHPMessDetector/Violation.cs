namespace Metrics.Integrations.Linters.Phpmd
{
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.Serialization;

    public class Violation
    {
        [XmlAttribute("beginline")]
        public int BeginLine { get; set; }
        [XmlAttribute("endline")]
        public int EndLine { get; set; }
        [XmlAttribute("rule")]
        public string Rule { get; set; }
        [XmlAttribute("ruleset")]
        public string RuleSet { get; set; }
        [XmlAttribute("package")]
        public string Package { get; set; }
        [XmlAttribute("class")]
        /// <summary>
        /// Class where Violation was found
        /// </summary>
        public string Class { get; set; }
        [XmlAttribute("method")]
        public string Method { get; set; }
        [XmlAttribute("priority")]
        public int Priority { get; set; }
        [XmlText]
        public string Description { get; set; }
    }
}
