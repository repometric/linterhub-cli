namespace Linterhub.Engine.Linters.jslint
{
    public class JsLintError : LinterFileModel.Error
    {
        public string Id { get; set; }
        public string Raw { get; set; }
        public string Code { get; set; }
        public string A { get; set; }
        public int Character { get; set; }
    }
}
