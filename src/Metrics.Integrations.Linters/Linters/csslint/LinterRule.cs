namespace Metrics.Integrations.Linters.csslint
{
    public class LinterRule : LinterFileModel.Rule
    {
        public string Description { get; set; }
        public string Browser { get; set; }
        public string GithubLink { get; set; }
    }
}