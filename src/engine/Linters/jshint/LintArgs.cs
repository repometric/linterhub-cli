namespace Linterhub.Engine.Linters.jshint
{
    public class LintArgs : ILinterArgs
    {

        public LintArgs()
        {
            JsHint = true;
            Reporter = "checkstyle";
        }

        /// <summary>
        /// Tested project path
        /// </summary>
        [Arg("", order: int.MaxValue, path:true)]
        public string TestPath { get; set; }

        /// <summary>
        /// Tool path
        /// </summary>
        [Arg("jshint", false, order: int.MinValue)]
        public bool JsHint { get; set; }

        /// <summary>
        /// Custom reporter (PATH|jslint|checkstyle|unix)
        /// </summary>
        [Arg("--reporter", order: 0)]
        public string Reporter { get; set; }

        /// <summary>
        /// Custom configuration file
        /// </summary>
        [Arg("--config", order: 0)]
        public string Config { get; set; }

        /// <summary>
        /// Comma-separate list of prerequisite (paths). E.g.
        /// files which include definitions of global variabls
        /// used throughout your project
        /// </summary>
        [Arg("--prereq", order: 0)]
        public string PreRequired { get; set; }

        /// <summary>
        /// Pass in a custom jshintignore file path
        /// </summary>
        [Arg("--exclude", order: 0)]
        public string Exclude { get; set; }

        /// <summary>
        /// Comma-separated list of file extensions to use (default is .js)
        /// </summary>
        [Arg("-e", order: 0)]
        public string ExtraExtensions { get; set; }

        /// <summary>
        /// Extract inline scripts contained in HTML (auto|always|never, default to never)
        /// </summary>
        [Arg("--extract", order: 0)]
        public string Extract { get; set; }
    }
}