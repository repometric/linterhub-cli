namespace Linterhub.Cli.Runtime
{
    using System.Threading.Tasks;

    public class LinterResult : RunResult
    {
        public Task<string> Task { get; set; }
    }
}
