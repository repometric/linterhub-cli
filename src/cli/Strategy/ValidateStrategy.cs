namespace Linterhub.Cli.Strategy
{
    using System.IO;
    using System.Runtime.InteropServices;
    using Linterhub.Engine.Exceptions;
    using Linterhub.Engine.Extensions;
    using Linterhub.Cli.Runtime;

    /// <summary>
    /// The 'validate input' strategy logic.
    /// </summary>
    public class ValidateStrategy : IStrategy
    {
        /// <summary>
        /// Run strategy.
        /// </summary>
        /// <param name="locator">The service locator.</param>
        /// <returns>Run results (null).</returns>
        public object Run(ServiceLocator locator)
        {
            var context = locator.Get<RunContext>();
            var ensure = locator.Get<Ensure>();
            context.Project = GetProjectPath(context.Project).NormalizePath();
            context.Linterhub = GetLinterhubPath(context.Linterhub).NormalizePath();
            context.PlatformConfig = GetPlatformConfigPath(context.PlatformConfig).NormalizePath();
            context.ProjectConfig = GetProjectConfigPath(context.ProjectConfig, context.Project).NormalizePath();

            ensure.ProjectExists();
            ensure.LinterhubExists();
            ensure.PlatformConfigExists();

            if (!string.IsNullOrEmpty(context.ProjectConfig))
            {
                ensure.ProjectConfigExists();
            }
            else
            {
                var possiblePath = Path.Combine(context.Project, ".linterhub.json");
                if (File.Exists(possiblePath))
                {
                    context.ProjectConfig = possiblePath;
                }
            }

            if (!string.IsNullOrEmpty(context.Directory))
            {
                context.Directory = Path.Combine(context.Project, context.Directory);
                ensure.DirectoryExists();
            }
/* 
            if (!string.IsNullOrEmpty(context.File) && !File.Exists(context.File))
            {
                context.File = Path.Combine(context.Project, context.File);
                ensure.FileExists();
            }
*/
            return null;
        }

        private string GetPlatformConfigPath(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                return path;
            }

            string file;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                file = "windows.json";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                file = "macos.json";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                file = "linux.json";
            }
            else
            {
                file = "default.json";
            }

            file = Path.Combine(Directory.GetCurrentDirectory(), "platform", file);
            return file;
        }

        private string GetLinterhubPath(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                return path;
            }

            return Path.Combine(Directory.GetCurrentDirectory(), "hub");
        }

        private string GetProjectPath(string path)
        {
            return string.IsNullOrEmpty(path) ? Directory.GetCurrentDirectory() : path;
        }

        private string GetProjectConfigPath(string path, string projectPath)
        {
            return !string.IsNullOrEmpty(path) ? path : string.Empty;
        }
    }
}