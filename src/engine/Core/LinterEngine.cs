namespace Linterhub.Engine
{
    using System.Collections;
    using System.IO;
    using Extensions;

    public class LinterEngine
    {
        public LinterFactory Factory { get; }

        public LinterEngine()
        {
            Factory = new LinterFactory();
        }

        public CmdWrapper.RunResults Run(string terminal, string command, string workingDirectory = null)
        {
            var wrapper = new CmdWrapper();
            var run = wrapper.RunExecutable(terminal, command, workingDirectory);
            return run;
        }

        public ILinterModel GetModel(string name, Stream stream, ILinterArgs args)
        {
            var linter = Factory.Create(name);
            var raw = linter.Parse(stream, args);
            var map = linter.Map(raw);
            return map;
        }

        public ILinterArgs GetArguments(string name, Stream stream)
        {
            var args = Factory.CreateArguments(name);
            return stream.DeserializeAsJson<ILinterArgs>(args.GetType());
        }
    }
}