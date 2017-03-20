namespace Linterhub.Engine.Runtime
{
    using System.Linq;
    using Linterhub.Engine.Schema;

    public class Installer
    {
        protected TerminalWrapper Terminal { get; }
        protected string IsInstalledCommand { get; }

        public Installer(TerminalWrapper terminal, string isInstalledCommand)
        {
            Terminal = terminal;
            IsInstalledCommand = isInstalledCommand;
        }

        public InstallResult IsInstalled(string program)
        {
            var command = string.Format(IsInstalledCommand, program);
            var result = Terminal.RunTerminal(command);
            return new InstallResult
            {
                Installed = result.ExitCode == 0
            };
        }

        public InstallResult Install(LinterSpecification specification)
        {
            var requirementsChecks = specification.Schema.Requirements.Select(requirement => 
            {
                var installCheck = IsInstalled(requirement.Package);
                if (installCheck.Installed)
                {
                    return installCheck;
                }

                return Install(requirement);
            });

            return requirementsChecks.FirstOrDefault(x => !x.Installed) ?? new InstallResult { Installed = true };
        }

        private InstallResult Install(LinterSchema.RequirementDefinition requirement)
        {
            if (requirement.Manager == "npm")
            {
                return InstallNpm(requirement.Package);
            }

            return new InstallResult
            {
                Installed = false,
                Message = $"Automatic installation for '{requirement.Package}' using '{requirement.Manager}' is not supported"
            };
        }

        private InstallResult InstallNpm(string package)
        {
            var command = $"npm install {package} -g";
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