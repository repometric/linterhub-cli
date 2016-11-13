namespace Linterhub.Engine.Linters.eslint
{
    public class LinterError : LinterFileModel.Error
    {
        public string NodeType { get; set; }
    }

}
