namespace Linterhub.Cli.Runtime
{
    using Linterhub.Engine;

    /// <summary>
    /// Represents run mode for CLI.
    /// </summary>
    public class RunResult
    {
        public string Name { get; set; }
        public ILinterModel Model { get; set; }
    }
}