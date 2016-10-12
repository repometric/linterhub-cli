namespace Metrics.Integrations.Linters.jslint
{
    public class LintArgs : ILinterArgs
    { 
        /// <summary>
        /// Tested project path
        /// </summary>
        public string TestPath { get; set; }

        /// <summary>
        /// Tested project path (in container)
        /// </summary>
        [Arg("", order: int.MaxValue)]
        public string TestPathDocker { get; set; }

        /// <summary>
        /// Your projcet Directory
        /// </summary>
        public string ProjectPath { get; set; }

        /// <summary>
        /// Tolerate assignment expressions
        /// </summary>
        [Arg("-ass", false)]
        public bool? Ass { get; set; }

        /// <summary>
        /// Tolerate bitwise operators
        /// </summary>
        [Arg("-bitwise", false)]
        public bool? Bitwise { get; set; }

        /// <summary>
        /// Assume a browser
        /// </summary>
        [Arg("-browser", false)]
        public bool? Browser { get; set; }

        /// <summary>
        /// Tolerate Google Closure idioms
        /// </summary>
        [Arg("-closure", false)]
        public bool? Closure { get; set; }

        /// <summary>
        /// Tolerate continue
        /// </summary>
        [Arg("-continue", false)]
        public bool? Continue  { get; set; }

        /// <summary>
        /// Tolerate debugger statements
        /// </summary>
        [Arg("-debug", false)]
        public bool? Debug { get; set; }

        /// <summary>
        ///  Assume console,alert, ...
        /// </summary>
        [Arg("-devel", false)]
        public bool? Devel { get; set; }

        /// <summary>
        /// Tolerate == and !=
        /// </summary>
        [Arg("-eqeq", false)]
        public bool? Eqeq { get; set; }

        /// <summary>
        /// Tolerate eval
        /// </summary>
        [Arg("-evil", false)]
        public bool? Evil { get; set; }

        /// <summary>
        /// Tolerate unfiltered for in
        /// </summary>
        [Arg("-forin", false)]
        public bool? Forin { get; set; }

        /// <summary>
        /// Strict white space indentation
        /// </summary>
        [Arg("-indent", false)]
        public bool? Indent { get; set; }

        /// <summary>
        ///  Maximum number of errors
        /// </summary>
        [Arg("-maxerr", false)]
        public bool? Maxerr { get; set; }

        /// <summary>
        ///  Maximum line length
        /// </summary>
        [Arg("-maxlen", false)]
        public bool? Maxlen { get; set; }

        /// <summary>
        ///  Tolerate uncapitalized constructors
        /// </summary>
        [Arg("-newcap", false)]
        public bool? Newcap { get; set; }

        /// <summary>
        /// Assume Node.js
        /// </summary>
        [Arg("-node", false)]
        public bool? Node { get; set; }

        /// <summary>
        /// Tolerate dangling underscore in identifiers
        /// </summary>
        [Arg("-nomen", false)]
        public bool? Nomen { get; set; }

        /// <summary>
        /// Stop on first error
        /// </summary>
        [Arg("-passfail", false)]
        public bool? Passfail { get; set; }

        /// <summary>
        /// Tolerate ++ and --
        /// </summary>
        [Arg("-plusplus", false)]
        public bool? Plusplus { get; set; }

        /// <summary>
        /// Declare additional predefined globals
        /// </summary>
        [Arg("-predef", false)]
        public bool? Predef  { get; set; }

        /// <summary>
        ///  Require all property names to be declared with /properties/
        /// </summary>
        [Arg("-properties", false)]
        public bool? Properties { get; set; }

        /// <summary>
        /// Tolerate . and [^...]. in /RegExp/
        /// </summary>
        [Arg("-regexp", false)]
        public bool? Regexp { get; set; }

        /// <summary>
        /// Assume Rhino
        /// </summary>
        [Arg("-rhino", false)]
        public bool? Rhino { get; set; }

        /// <summary>
        ///  Tolerate missing 'use strict' pragma
        /// </summary>
        [Arg("-sloppy", false)]
        public bool? Sloppy { get; set; }

        /// <summary>
        /// Tolerate stupidity (typically, use of sync functions)
        /// </summary>
        [Arg("-stupid", false)]
        public bool? Stupid { get; set; }

        /// <summary>
        /// Tolerate inefficient subscripting
        /// </summary>
        [Arg("-sub", false)]
        public bool? Sub { get; set; }

        /// <summary>
        /// Tolerate TODO comments
        /// </summary>
        [Arg("-todo", false)]
        public bool? Todo { get; set; }

        /// <summary>
        /// Tolerate unused parameters
        /// </summary>
        [Arg("-unparam", false)]
        public bool? Unparam { get; set; }

        /// <summary>
        /// Tolerate many var statements per function
        /// </summary>
        [Arg("-vars", false)]
        public bool? Vars { get; set; }

        /// <summary>
        ///  Tolerate messy white space
        /// </summary>
        [Arg("-white", false)]
        public bool? White { get; set; }

        /// <summary>
        /// Tolerate no space in anonymous function definition
        /// </summary>
        [Arg("-anon", false)]
        public bool? Anon { get; set; }

        /// <summary>
        /// Tolerate ECMAScript 5 syntax
        /// </summary>
        [Arg("-es5", false)]
        public bool? Es5 { get; set; }

        /// <summary>
        /// Tolerate variables used before declaration
        /// </summary>
        [Arg("-undef", false)]
        public bool? Undef { get; set; }

        /// <summary>
        /// Tolerate HTML event handlers
        /// </summary>
        [Arg("-on", false)]
        public bool? On { get; set; }

        /// <summary>
        /// Assume existence of Windows globals
        /// </summary>
        [Arg("-windows", false)]
        public bool? Windows { get; set; }
    }
}
