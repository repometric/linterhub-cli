namespace Linterhub.Cli.Runtime
{
    using Engine;

    public class ValidationContext
    {
        public ProjectConfig ProjectConfig { get; set; }
        public ArgMode ArgMode { get; set; }
        public bool ExistFileConfig { get; set; }
        public bool IsLinterSpecified { get; set; }
        public string PathFileConfig { get; set; }
        public string WorkDir { get; set; }
        public string Path { get; set; }
    }
}
