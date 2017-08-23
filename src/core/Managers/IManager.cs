namespace Linterhub.Core.Managers
{
    using static Schema.EngineSchema;
    using InstallResult = Schema.EngineVersionSchema.ResultType;

    /// <summary>
    /// The package manager interface.
    /// </summary>
    public interface IManager
    {
        RequirementType.ManagerType GetManagerType();

        InstallResult Install(string packageName, string installationPath = null, string version = null);

        InstallResult CheckInstallation(string packageName, string installationPath = null);

        string LocallyExecution(string packageName);
    }
}
