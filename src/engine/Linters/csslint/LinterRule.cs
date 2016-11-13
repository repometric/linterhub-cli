namespace Linterhub.Engine.Linters.csslint
{
    public class LinterRule : LinterFileModel.Rule
    {
        /// <summary>
        ///  Small description of the Rule
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///  For which browsers this rule works
        /// </summary>
        public string Browser { get; set; }

        /// <summary>
        ///  Link to GitHub Page with more information
        /// </summary>
        public string GithubLink { get; set; }
    }
}