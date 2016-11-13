namespace Linterhub.Engine.Linters.eslint
{
    public class LintArg
    {
        /// <summary>
        /// Tested project path
        /// </summary>
        public string TestPath { get; set; }

        /// <summary>
        /// Your projcet Directory
        /// </summary>
        public string ProjectPath { get; set; }

        /// <summary>
        /// Required field for work EsLint 
        /// </summary>
        [Arg("eslint")]
        public bool EsLint { get; set; }

        /// <summary>
        /// Check Format File (Example: **/*.js)
        /// </summary>
        [Arg(order:int.MinValue + 1)]
        public string CheckFormatFile { get; set; }
        /// <summary>
        /// Use configuration from this file or shareable
        /// </summary>
        [Arg("-c")]
        public string ConfigPath { get; set; }


        /// <summary>
        /// Disable use of configuration from .eslintrc
        /// </summary>
        [Arg("--no-eslintrc", false)]
        public bool? NoEslintc { get; set; }

        /// <summary>
        /// Specify environments
        /// </summary>
        [Arg("--env")]
        public string Environments { get; set; }


        /// <summary>
        /// Specify JavaScript file extensions - default: .js
        /// </summary>
        [Arg("--ext")]
        public string Extensions { get; set; }

        /// <summary>
        ///  Define global variables
        /// </summary>
        [Arg("--global")]
        public string Global { get; set; }

        /// <summary>
        /// Only check changed files - default: false
        /// </summary>
        [Arg("--cache", false)]
        public bool? Cache { get; set; }

        /// <summary>
        /// Path to the cache file - default: .eslintcache. Deprecated: use --cache-location 
        /// </summary>
        [Arg("--cache-file")]
        public string CacheFile { get; set; }

        /// <summary>
        /// Path to the cache file or directory.
        /// </summary>
        [Arg("--cache-location")]
        public string CacheLocation { get; set; }

        /// <summary>
        /// Use additional rules from this directory
        /// </summary>
        [Arg("--rulesdir")]
        public string RulesDir { get; set; }

        /// <summary>
        /// Specify plugins
        /// </summary>
        [Arg("--plugin")]
        public string Plugin { get; set; }

        /// <summary>
        /// Specify rules
        /// </summary>
        [Arg("--rule")]
        public string Rule { get; set; }

        /// <summary>
        /// Specify path of ignore file
        /// </summary>
        [Arg("--ignore-path")]
        public string IgnorePath { get; set; }

        /// <summary>
        /// Disable use of ignore files and patterns
        /// </summary>
        [Arg("--no-ignore", false)]
        public bool? NoIgnore { get; set; }

        /// <summary>
        /// Patterns of files to ignore (in addition to those in .eslintignore)
        /// </summary>
        [Arg("--ignore-pattern")]
        public string IgnorePattern { get; set; }

        /// <summary>
        /// Lint code provided on STDIN - default: false
        /// </summary>
        [Arg("--stdin", false)]
        public bool? Stdin { get; set; }

        /// <summary>
        /// Specify filename to process STDIN as
        /// </summary>
        [Arg("--stdin-filename")]
        public string StdinFilename { get; set; }

        /// <summary>
        ///  Report errors only - default: false
        /// </summary>
        [Arg("--quiet", false)]
        public bool? Quiet { get; set; }

        /// <summary>
        /// Number of warnings to trigger nonzero exit code - default: -1
        /// </summary>
        [Arg("--max-warnings")]
        public int? MaxWarnings { get; set; }

        /// <summary>
        /// Automatically fix problems
        /// </summary>
        [Arg("--fix", false)]
        public bool? Fix { get; set; }

        /// <summary>
        /// Output debugging information
        /// </summary>
        [Arg("--debug", false)]
        public bool? Debug { get; set; }

        /// <summary>
        /// Output file format (Example: jslint-xml)
        /// </summary>
        [Arg("-f")]
        public string OutputFormatFile { get; set; }
    }
}
