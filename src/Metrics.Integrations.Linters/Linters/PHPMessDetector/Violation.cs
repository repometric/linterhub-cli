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
        /// <summary>
        /// Ruleset that throw Violation
        /// </summary>
        public string RuleSet { get; set; }

        [XmlAttribute("package")]
        /// <summary>
        /// Package where Violation was found
        /// </summary>
        public string Package { get; set; }

        [XmlAttribute("class")]
        /// <summary>
        /// Class where Violation was found
        /// </summary>
        public string Class { get; set; }

        [XmlAttribute("method")]
        /// <summary>
        /// Method where Violation was found
        /// </summary>
        public string Method { get; set; }

        [XmlAttribute("priority")]
        /// <summary>
        /// Importance of the Violation
        /// </summary>
        public int Priority { get; set; }

        [XmlText]
        /// <summary>
        /// Detailed description of the error
        /// </summary>
        public string Description { get; set; }
    }
}
