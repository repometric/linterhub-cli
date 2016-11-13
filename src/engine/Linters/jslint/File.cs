namespace Linterhub.Engine.Linters.jslint
{
    using System.Collections.Generic;

    public class File
    {
        public string Path { get; set; }
        public List<Error> Errors { get; set; }
    }
}
