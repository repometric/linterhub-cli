namespace Metrics.Integrations.Linters.jslint
{
    using System.Collections.Generic;

    public class File
    {
        public string path { get; set; }
        public List<Error> error { get; set; }
    }

    public class Error
    {
        public string id { get; set; }
        public string raw { get; set; }
        public string code { get; set; }
        public string evidence { get; set; }
        public int line { get; set; }
        public int character { get; set; }
        public string a { get; set; }
        public string b { get; set; }
        public string reason { get; set; }
    }
}
