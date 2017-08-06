namespace Linterhub.Core.Runtime
{
    using System.Collections.Generic;
    using System.Linq;
    using Schema;
    using Managers;
    using InstallResult = Schema.EngineVersionSchema.ResultType;

    public class Installer
    {
        protected TerminalWrapper Terminal { get; }
        protected ManagerWrapper Managers { get; }

        public Installer(TerminalWrapper terminal, ManagerWrapper managerWrapper)
        {
            Terminal = terminal;
            Managers = managerWrapper;
        }

        private InstallResult cantInstall(EngineSchema.RequirementType requirement)
        {
            return new InstallResult
            {
                Name = requirement.Package,
                Installed = false,
                Message = $"Automatic installation for '{requirement.Package}' using '{requirement.Manager}' is not supported"
            };
        }

        public InstallResult Install(EngineSpecification specification, string installationPath = null, string version = null)
        {
            var mainPackage = specification.Schema.Requirements.First(x => x.Package == specification.Schema.Name);

            var result = new InstallResult()
            {
                Name = specification.Schema.Name,
                Packages = new List<InstallResult>(specification.Schema.Requirements
                    .Select(requirement =>
                {
                    var manager = Managers.get(requirement.Manager);

                    if (manager == null)
                    {
                        return cantInstall(requirement);
                    }

                    var installCheck = manager.CheckInstallation(requirement.Package, installationPath);

                    var insResult = installCheck.Installed ?
                                    installCheck :
                                    manager.Install(requirement.Package, installationPath, mainPackage.Package == requirement.Package ? version : null);

                    return insResult ?? cantInstall(requirement);
                }))
            };

            result.Installed = result.Packages.All((x) => x.Installed);

            var mainResult = result.Packages.Where(x => x.Name == mainPackage.Package).First();

            result.Version = mainResult.Version;
            result.Message = mainResult.Message;

            result.Packages.Remove(mainResult);

            return result;
        }

    }

}