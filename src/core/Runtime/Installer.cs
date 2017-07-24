namespace Linterhub.Core.Runtime
{
    using System.Collections.Generic;
    using System.Linq;
    using Schema;
    using static Schema.EngineSchema;
    using System.Text.RegularExpressions;
    using Extensions;
    using InstallResult = Schema.EngineVersionSchema.ResultType;

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
            var command = "";
            CmdWrapper.Result checkResult = null;
            switch (requirement.Manager)
            {
                case RequirementType.ManagerType.npm:
                    command = string.Format("npm list -g --depth=0");
                    checkResult = Terminal.RunTerminal(command);
                    var regex = new Regex($"\\s{requirement.Package}\\@(.*)", RegexOptions.IgnoreCase);
                    var match = regex.Match(checkResult.Output.ToString());
                    if(match.Success)
                    {
                        return new InstallResult
                        {
                            Name = requirement.Package,
                            Installed = true,
                            Version = Deserialize.RemoveNewline(match.Groups[1].Value),
                            Message = ""
                        };
                    }
                    break;
            }

            command = string.Format("{0} --version", requirement.Package);
            checkResult = Terminal.RunTerminal(command);
            return new InstallResult
            {
                Name = requirement.Package,
                Installed = checkResult.ExitCode == 0,
                Version = Deserialize.RemoveNewline(checkResult.Output.ToString()),
                Message = Deserialize.RemoveNewline(checkResult.Error.ToString())
            };
        }

        public InstallResult Install(EngineSpecification specification)
        {
            var result = new InstallResult()
            {
                Name = specification.Schema.Name,
                Packages = new List<InstallResult>(specification.Schema.Requirements
                    .Where(x => x.Manager != RequirementType.ManagerType.system && x.Package != specification.Schema.Name)
                    .Select(requirement =>
                {
                    var installCheck = IsInstalled(requirement);
                    return installCheck.Installed ?? false ? installCheck : Install(requirement, specification.Schema.Name, specification.Schema.Version.Package);
                }))
            };

            result.Installed = result.Packages.All((x) => x.Installed ?? false);
            
            var mainPackage = IsInstalled(specification.Schema.Requirements.First(x => x.Package == specification.Schema.Name));
            result.Version = mainPackage.Version;
            result.Message = mainPackage.Message;

            return result;
        }

        private InstallResult Install(RequirementType requirement, string Engine, string Version)
        {
            var command = "";
            switch (requirement.Manager)
            {
                case RequirementType.ManagerType.npm:
                    command = $"npm install -g {requirement.Package}";
                    if (Engine == requirement.Package)
                        command += "@" + Version;
                    break;
                case RequirementType.ManagerType.pip:
                    command = $"pip install {requirement.Package}";
                    break;
                default:
                    return new InstallResult
                    {
                        Name = requirement.Package,
                        Installed = false,
                        Message = $"Automatic installation for '{requirement.Package}' using '{requirement.Manager}' is not supported"
                    };
            }

            var result = Terminal.RunTerminal(command);
            return IsInstalled(requirement);
        }
    }

}