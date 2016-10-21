namespace Metrics.Integrations.Linters.eslint
{

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class jslint
    {

        private jslintFile[] fileField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("file")]
        public jslintFile[] file
        {
            get
            {
                return this.fileField;
            }
            set
            {
                this.fileField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class jslintFile
    {

        private jslintFileIssue[] issueField;

        private string nameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("issue")]
        public jslintFileIssue[] issue
        {
            get
            {
                return this.issueField;
            }
            set
            {
                this.issueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class jslintFileIssue
    {

        private ushort lineField;

        private ushort charField;

        private string evidenceField;

        private string reasonField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort line
        {
            get
            {
                return this.lineField;
            }
            set
            {
                this.lineField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort @char
        {
            get
            {
                return this.charField;
            }
            set
            {
                this.charField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string evidence
        {
            get
            {
                return this.evidenceField;
            }
            set
            {
                this.evidenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string reason
        {
            get
            {
                return this.reasonField;
            }
            set
            {
                this.reasonField = value;
            }
        }
    }


}
