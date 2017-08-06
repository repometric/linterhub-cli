namespace Linterhub.Core.Managers
{
    using Extensions;
    using Runtime;
    using System.Text.RegularExpressions;
    using static Schema.EngineSchema;
    using InstallResult = Schema.EngineVersionSchema.ResultType;

    public class SystemManager : IManager
    {
        protected TerminalWrapper Terminal { get; }

        public RequirementType.ManagerType GetManagerType()
        {
            return RequirementType.ManagerType.system;
        }

        public InstallResult Install(string packageName, string installationPath = null, string version = null)
        {
            return CheckInstallation(packageName, installationPath);
        }

        public InstallResult CheckInstallation(string packageName, string installationPath = null)
        {
            var command = string.Format("{0} --version", packageName);

            var checkResult = Terminal.RunTerminal(command);

            return new InstallResult
            {
                Name = packageName,
                Installed = checkResult.ExitCode == 0,
                Version = Deserialize.RemoveNewline(checkResult.Output.ToString()),
                Message = Deserialize.RemoveNewline(checkResult.Error.ToString())
            };
        }

        public SystemManager(TerminalWrapper terminal)
        {
            Terminal = terminal;
        }
    }
}
