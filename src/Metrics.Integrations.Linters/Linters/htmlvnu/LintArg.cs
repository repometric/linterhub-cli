namespace Metrics.Integrations.Linters.htmlvnu
{
    using System.IO;

    public class LintArg
    {
        /// <summary>
        /// Path to HtmlVNU Application
        /// </summary>
        public string ApplicationPath { get; set; }

        /// <summary>
        /// Path to temp File
        /// </summary>
        public DirectoryInfo TempPath { get; set; }

        /// <summary>
        /// Your projcet Directory
        /// </summary>
        [Arg(order:int.MaxValue)]
        public string ProjectPath { get; set; }

        /// <summary>
        /// Specifies that only error-level messages and non-document-error messages are reported(so that warnings and info messages are not reported).
        /// </summary>
        [Arg("--errors-only", false)]
        public bool? ErrorOnly { get; set; }

        /// <summary>
        /// Forces all documents to be be parsed in buffered mode instead of streaming mode(causes some parse errors to be treated as non-fatal document errors instead of as fatal document errors).
        /// </summary>
        [Arg("--no-stream", false)]
        public bool? NoStream { get; set; }

        /// <summary>
        /// Forces any *.xhtml or *.xht documents to be parsed using the HTML parser.
        /// </summary>
        [Arg("--html", false)]
        public bool? Html { get; set; }

        /// <summary>
        /// Disables language detection, so that documents are not checked for missing or mislabeled html[lang] attributes.
        /// </summary>
        [Arg("--no-langdetect", false)]
        public bool? NoLangdetect { get; set; }

        /// <summary>
        /// Skip documents that don’t have *.html, *.htm, *.xhtml, or *.xht extensions.
        /// </summary>
        [Arg("--skip-non-html", false)]
        public bool? SkipNonHtml { get; set; }

        /// <summary>
        /// Specifies "verbose" output. (Currently this just means that the names of files being checked are written to stdout.)
        /// </summary>
        [Arg("--verbose", false)]
        public bool? Verbose { get; set; }
    }
}
