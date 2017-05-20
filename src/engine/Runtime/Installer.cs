namespace Linterhub.Engine.Runtime
{
    using System.Linq;
    using Linterhub.Engine.Schema;
    using static Schema.EngineSchema;

    public class Installer
    {
        protected TerminalWrapper Terminal { get; }
        protected string IsInstalledCommand { get; }

        public Installer(TerminalWrapper terminal, string isInstalledCommand)
        {
            Terminal = terminal;
            IsInstalledCommand = isInstalledCommand;
        }

        public InstallResult IsInstalled(RequirementType requirement)
        {
            // var command = string.Format(IsInstalledCommand, package);
            // hot fix
            var command = string.Format("{0} --version", requirement.Package);
            var checkResult = Terminal.RunTerminal(command);
            return new InstallResult
            {
                Installed = checkResult.ExitCode == 0,
                Message = checkResult.Output.ToString().Replace("\r","").Replace("\n","")
            };
        }

        public InstallResult Install(LinterSpecification specification)
        {
            var requirementsChecks = specification.Schema.Requirements.Select(requirement =>
            {
                var installCheck = IsInstalled(requirement);
                if (installCheck.Installed)
                {
                    return installCheck;
                }

                return Install(requirement, specification.Schema.Name, specification.Schema.Version.Package);
            });

            return requirementsChecks.FirstOrDefault(x => !x.Installed) ?? new InstallResult { Installed = true };
        }

        private InstallResult Install(RequirementType requirement, string Linter, string Version)
        {
            var command = "";
            switch (requirement.Manager)
            {
                case RequirementType.ManagerType.npm:
                    command = $"npm install -g {requirement.Package}";
                    if (Linter == requirement.Package)
                        command += "@" + Version;
                    break;
                case RequirementType.ManagerType.pip:
                    command = $"pip install {requirement.Package}";
                    break;
                default:
                    return new InstallResult
                    {
                        Installed = false,
                        Message = $"Automatic installation for '{requirement.Package}' using '{requirement.Manager}' is not supported"
                    };
            }

            var result = Terminal.RunTerminal(command);
            var stdError = result.Error.ToString().Trim();
            var isInstalled = result.ExitCode == 0 && string.IsNullOrEmpty(stdError) && result.RunException == null;
            return new InstallResult
            {
                Installed = isInstalled,
                Message = isInstalled ? null : stdError
            };
        }

        public class InstallResult
        {
            public bool Installed { get; set; }
            public string Message { get; set; }
        }
    }

}