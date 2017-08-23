namespace Linterhub.Core.Managers
{
    using System;
    using Runtime;
    using static Schema.EngineSchema;
    using InstallResult = Schema.EngineVersionSchema.ResultType;

    public class PipManager : IManager
    {
        protected TerminalWrapper Terminal { get; }

        public RequirementType.ManagerType GetManagerType()
        {
            return RequirementType.ManagerType.pip;
        }

        public InstallResult Install(string packageName, string installationPath = null, string version = null)
        {
            // command = $"pip install {requirement.Package}";
            return null;
        }

        public InstallResult CheckInstallation(string packageName, string installationPath = null)
        {
            // TODO add support
            return null;
        }

        public string LocallyExecution(string packageName)
        {
            throw new NotImplementedException();
        }

        public PipManager(TerminalWrapper terminal)
        {
            Terminal = terminal;
        }
    }
}
