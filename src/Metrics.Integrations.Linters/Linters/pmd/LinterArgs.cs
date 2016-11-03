namespace Metrics.Integrations.Linters.pmd
{
    using System.IO;

    public class LinterArgs
    {
        /// <summary>
        /// Path to PMD Application
        /// </summary>
        public string ApplicationPath { get; set; }

        /// <summary>
        /// Path to temp File
        /// </summary>
        public DirectoryInfo TempPath { get; set; }

        /// <summary>
        /// Sets the number of threads used by PMD. Default is `1`. Set threads to '0' to disable multi-threading processing.
        /// </summary>
        [Arg("-t")]
        public int? WorkingThreads { get; set; }

        /// <summary>
        /// Rule priority threshold; rules with lower priority than configured here won't be used. Default is `5` - which is the lowest priority.
        /// </summary>
        [Arg("-min")]
        public int? MinImumpriority { get; set; }

        /// <summary>
        /// Your projcet Directory
        /// </summary>
        [Arg("-d")]
        public string ProjectPath { get; set; }

        /// <summary>
        /// Comma separated list of ruleset names to use
        /// </summary>
        [Arg("-R", order: int.MaxValue)]
        public string RulesetsPath { get; set; }

        /// <summary>
        /// Specify a language PMD should use.
        /// </summary>
        [Arg("-l")]
        public Langues? Lange { get; set; }

        /// <summary>
        /// By default PMD exits with status 4 if violations are found. Disable this option with '-failOnViolation false' to exit with 0 instead and just write the report.
        /// </summary>
        [Arg("-failOnViolation true", false)]
        public bool? FailOnViolation { get; set; }

        /// <summary>
        /// Debug mode.Prints more log output.
        /// </summary>
        [Arg("-D", false)]
        public bool? Debug { get; set; }

        /// <summary>
        /// Benchmark mode - output a benchmark report upon completion; defaults to System.err
        /// </summary>
        [Arg("-b", false)]
        public bool? Benchmark { get; set; }

        /// <summary>
        ///  Performs a stress test.no	
        /// </summary>
        [Arg("-S", false)]
        public bool? Stress { get; set; }

        /// <summary>
        /// Prints shortened filenames in the report.
        /// </summary>
        [Arg("-shortnames", false)]
        public bool? Shortnames { get; set; }

        /// <summary>
        /// Report should show suppressed rule violations.
        /// </summary>
        [Arg("-showsuppressed", false)]
        public bool? Showsuppressed { get; set; }

        /// <summary>
        ///  Specifies the string that marks the line which PMD should ignore; default is `NOPMD`.
        /// </summary>
        [Arg("-suppressmarker")]
        public string Suppressmarker { get; set; }

        /// <summary>
        ///  Database URI for sources.If this is given, then you don't need to provide `-dir`.	Only plsql
        /// </summary>
        [Arg("-u")]
        public string DatabaseUri { get; set; }

        /// <summary>
        /// Specifies the character set encoding of the source code files PMD is reading (i.e.UTF-8). Default is `UTF-8`.
        /// </summary>
        [Arg("-e")]
        public string Endcoding { get; set; }

        /// <summary>
        /// Specifies the classpath for libraries used by the source code. This is used by the type resolution.Alternatively a `file://` URL to a text file containing path elements on consecutive lines can be specified.
        /// </summary>
        [Arg("-auxclasspath")]
        public string Auxclasspath{ get; set; }

        public enum Langues
        {
            java,
            ecmascript,
            jsp,
            plsql,
            vm,
            xml,
            xls
        }
    }
}


