namespace Linterhub.Engine.Linters.prospector
{
    public class LintArgs : ILinterArgs
    {

        public LintArgs()
        {
            ToolPath = "prospector";
            ReportType = "json";
        }

        /// <summary>
        /// Tool path
        /// </summary>
        [Arg("", order: int.MinValue)]
        public string ToolPath { get; set; }

        /// <summary>
        /// Indicate which format to use for output.
        /// </summary>
        [Arg("-o", separator: "=", order: int.MaxValue)]
        public string ReportType { get; set; }

        /// <summary>
        /// A list of one or more libraries or frameworks that the project uses.
        /// Possible values are: django, celery, flask.
        /// This will be autodetected by default, but if autodetection doesn’t work,
        /// manually specify them using this flag.
        /// </summary>
        [Arg("-u", separator: "=", order: int.MaxValue)]
        public string UsesLibraries { get; set; }

        /// <summary>
        /// The maximum line length allowed.
        /// This will be set by the strictness if no value is explicitly specified.
        /// </summary>
        [Arg("--max-line-length", separator: "=", order: int.MaxValue)]
        public string MaxLineLength { get; set; }

        /// <summary>
        /// A list of tools to run. This lets you set exactly which tools to run.
        /// Possible values are:
        /// dodgy, frosted, mccabe, pep257, pep8, profile-validator, pyflakes, pylint, pyroma, vulture.
        /// By default, the following tools will be run:
        /// dodgy, mccabe, pep257, pep8, profile-validator, pyflakes, pylint
        /// </summary>
        [Arg("-t", separator: "=", order: int.MaxValue)]
        public string Tools { get; set; }

        /// <summary>
        /// A list of tools to run in addition to the default tools.
        /// Possible values are dodgy, frosted, mccabe, pep257, pep8, profile-validator, pyflakes,
        /// pylint, pyroma, vulture.
        /// </summary>
        [Arg("-w", separator: "=", order: int.MaxValue)]
        public string WithTools { get; set; }

        /// <summary>
        /// A list of tools that should not be run.
        /// Useful to turn off only a single tool from the defaults.
        /// Possible values are dodgy, frosted, mccabe, pep257, pep8,
        /// profile-validator, pyflakes, pylint, pyroma, vulture.
        /// </summary>
        [Arg("-W", separator: "=", order: int.MaxValue)]
        public string WithoutTools { get; set; }

        /// <summary>
        /// The list of profiles to load.
        /// A profile is a certain ‘type’ of behaviour for prospector,
        /// and is represented by a YAML configuration file.
        /// Either a full path to the YAML file describing the profile must be provided,
        /// or it must be on the profile path (see ProfilePath)
        /// </summary>
        [Arg("-P", separator: "=", order: int.MaxValue)]
        public string Profiles { get; set; }

        /// <summary>
        /// Additional paths to search for profile files.
        /// By default this is the path that prospector will check, and a directory called ”.prospector”
        /// in the path that prospector will check.
        /// </summary>
        [Arg("--profile-path", separator: "=", order: int.MaxValue)]
        public string ProfilePath { get; set; }

        /// <summary>
        /// How strict the checker should be. 
        /// This affects how harshly the checker will enforce coding guidelines. 
        /// The default value is “medium”, possible values are “veryhigh”, “high”, “medium”, “low” and “verylow”.
        /// Possible choices: veryhigh, high, medium, low, verylow
        /// </summary>
        [Arg("--strictness", separator: "=", order: int.MaxValue)]
        public string Strictness { get; set; }

        /// <summary>
        /// The path to a pylintrc file to use to configure pylint.
        /// Prospector will find .pylintrc files in the root of the project,
        /// but you can use this option to specify manually where it is.
        /// </summary>
        [Arg("--pylint-config-file", separator: "=", order: int.MaxValue)]
        public string PylintConfig { get; set; }

        /// <summary>
        /// A list of paths to ignore, as a list of regular expressions.
        /// Files and folders will be ignored if their full path contains any of these patterns.
        /// </summary>
        [Arg("--ignore-patterns", separator: "=", order: int.MaxValue)]
        public string IgnorePatterns { get; set; }

        /// <summary>
        /// A list of file or directory names to ignore.
        /// If the complete name matches any of the items in this list, the file or directory
        /// (and all subdirectories) will be ignored.
        /// </summary>
        [Arg("--ignore-paths", separator: "=", order: int.MaxValue)]
        public string IgnorePaths { get; set; }

        /// <summary>
        /// Tested project path (in container)
        /// </summary>
        [Arg("", order: 0)]
        public string TestPath { get; set; }

    }
}