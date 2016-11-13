namespace Linterhub.Engine.Linters.jslint
{
    public class Error
    {
        public string Id { get; set; }
        public string Raw { get; set; }
        public string Code { get; set; }
        public string Evidence { get; set; }
        public int Line { get; set; }
        public int Character { get; set; }
        public string A { get; set; }
        public string B { get; set; }
        public string Reason { get; set; }
    }
}
