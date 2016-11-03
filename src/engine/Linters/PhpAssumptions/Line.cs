namespace Linterhub.Engine.Linters.PhpAssumptions
{
    using System.Xml;
    using System.Xml.Serialization;

    public class Line
    {
        /// <summary>
        ///  Contains code with weak assumptions
        /// </summary>
        [XmlAttribute("message")]
        public string Message { get; set; }

        /// <summary>
        ///  Number of this Line in a file
        /// </summary>
        [XmlAttribute("number")]
        public string LineNumber { get; set; }
    }
}
