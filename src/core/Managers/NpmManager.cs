namespace Linterhub.Core.Managers
{
    using Extensions;
    using Runtime;
    using System.Text.RegularExpressions;
    using static Schema.EngineSchema;
    using InstallResult = Schema.EngineVersionSchema.ResultType;

    public class NpmManager : IManager
    {
        protected TerminalWrapper Terminal { get; }

        public RequirementType.ManagerType GetManagerType()
        {
            return RequirementType.ManagerType.npm;
        }

        public InstallResult Install(string packageName, string installationPath = null, string version = null)
        {
            var command = "";
            command = string.Format("npm install -g {0}{1}", packageName, version != null ? "@" + version : "");

            Terminal.RunTerminal(command);

            return CheckInstallation(packageName, installationPath);
        }

        public InstallResult CheckInstallation(string packageName, string installationPath = null)
        {
            var command = string.Format("npm list {0} --depth=0", installationPath == null ? "-g" : "");
            var checkResult = Terminal.RunTerminal(command);
            var regex = new Regex($"\\s{packageName}\\@(.*)", RegexOptions.IgnoreCase);
            var match = regex.Match(checkResult.Output.ToString());
            return new InstallResult
            {
                Name = packageName,
                Installed = match.Success,
                Version = Deserialize.RemoveNewline(match.Groups[1].Value),
                Message = string.Empty
            };
        }

        public NpmManager(TerminalWrapper terminal)
        {
            Terminal = terminal;
        }
    }
}
