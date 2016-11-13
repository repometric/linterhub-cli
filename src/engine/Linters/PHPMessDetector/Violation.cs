namespace Linterhub.Engine.Linters.Phpmd
{
    using System.Xml.Serialization;

    public class Violation
    {
        [XmlAttribute("beginline")]
        public int BeginLine { get; set; }

        [XmlAttribute("endline")]
        public int EndLine { get; set; }

        [XmlAttribute("rule")]
        public string Rule { get; set; }

        /// <summary>
        /// Ruleset that throws Violation
        /// </summary>
        [XmlAttribute("ruleset")]
        public string RuleSet { get; set; }

        /// <summary>
        /// Package where Violation was found
        /// </summary>
        [XmlAttribute("package")]
        public string Package { get; set; }

        /// <summary>
        /// Class where Violation was found
        /// </summary>
        [XmlAttribute("class")]
        public string Class { get; set; }

        /// <summary>
        /// Method where Violation was found
        /// </summary>
        [XmlAttribute("method")]
        public string Method { get; set; }

        /// <summary>
        /// Importance of the Violation
        /// </summary>
        [XmlAttribute("priority")]
        public int Priority { get; set; }

        /// <summary>
        /// Detailed description of the error
        /// </summary>
        [XmlText]
        public string Description { get; set; }
    }
}
