namespace Linterhub.Engine.Linters.Phpcf
{
    public class LintArgs : ILinterArgs
    {
        /// <summary>
        /// Tool path
        /// </summary>
        [Arg("", separator: "", order: int.MinValue)]
        public string ToolPath { get; set; }

        /// <summary>
        ///  Sets max size of php file.
        ///  If size of file is above this value, file will be skipped [default: 1mb]
        /// </summary>
        [Arg("--max-size", separator: " ", order: 0)]
        public string MaxSize { get; set; }

        /// <summary>
        ///  Target php version [default: 5.6]
        /// </summary>
        [Arg("--target", separator: " ", order: 0)]
        public string PhpVersion { get; set; }

        /// <summary>
        /// Tested project path
        /// </summary>
        [Arg("", separator: "", order: int.MaxValue)]
        public string TestPath { get; set; }
    }
}