namespace Metrics.Integrations.Extensions
{
	using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text;

    /// <summary>
    /// The cmd wrapper.
    /// </summary>
    public class CmdWrapper
    {
        /// <summary>
        /// The run executable.
        /// </summary>
        /// <param name="executablePath">The executable path.</param>
        /// <param name="arguments">The arguments.</param>
        /// <param name="workingDirectory">The working directory.</param>
        /// <param name="waitTimeout">The amount of time, in milliseconds, to wait for the associated process to exit.</param>
        /// <returns>The <seealso cref="RunResults"/>.</returns>
        public RunResults RunExecutable(string executablePath, string arguments, string workingDirectory = null, int waitTimeout = -1)
        {
            var runResults = new RunResults
            {
                Output = new StringBuilder(),
                Error = new StringBuilder(),
                RunException = null
            };

            try
            {
                if (!File.Exists(executablePath))
                {
                    throw new ArgumentException("Invalid executable path.", nameof(executablePath));
                }

                using (var process = new Process())
                {
                    process.StartInfo.FileName = executablePath;
                    process.StartInfo.Arguments = arguments;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.OutputDataReceived += (o, e) => runResults.Output.Append(e.Data).Append(Environment.NewLine);
                    process.ErrorDataReceived += (o, e) => runResults.Error.Append(e.Data).Append(Environment.NewLine);
                    if (!string.IsNullOrEmpty(workingDirectory))
                    {
                        process.StartInfo.WorkingDirectory = workingDirectory;
                    }

                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                    process.WaitForExit(waitTimeout);
                    runResults.ExitCode = process.ExitCode;
                }
            }
            catch (Exception e)
            {
                runResults.RunException = e;
                runResults.ExitCode = -1;
            }

            return runResults;
        }

        /// <summary>
        /// The cmd run results.
        /// </summary>
        public class RunResults
        {
            /// <summary>
            /// Gets the exit code.
            /// </summary>
            public int ExitCode { get; internal set; }

            /// <summary>
            /// Gets the run exception.
            /// </summary>
            public Exception RunException { get; internal set; }

            /// <summary>
            /// Gets output content.
            /// </summary>
            public StringBuilder Output { get; internal set; }

            /// <summary>
            /// Gets error content.
            /// </summary>
            public StringBuilder Error { get; internal set; }
        }
    }
}
