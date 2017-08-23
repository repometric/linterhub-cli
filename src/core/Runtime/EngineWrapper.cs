namespace Linterhub.Core.Runtime
{
    using System.IO;
    using Exceptions;
    using Schema;
    using System.Collections.Generic;
    using System.Linq;
    using Utils;
    using Factory;
    using Result = Schema.LinterhubOutputSchema.ResultType;
    using System.Threading.Tasks;
    using Managers;
    using System;

    /// <summary>
    /// The engine wrapper.
    /// </summary>
    public class EngineWrapper
    {
        /// <summary>
        /// Gets the terminal wrapper.
        /// </summary>
        protected TerminalWrapper Terminal { get; private set; }

        protected ManagerWrapper Managers { get; private set; }

        /// <summary>
        /// Gets the command factory.
        /// </summary>
        protected CommandFactory CommandFactory { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <seealso cref="EngineWrapper"/>.
        /// </summary>
        /// <param name="terminal">The terminal wrapper.</param>
        /// <param name="commandFactory">The command factory.</param>
        public EngineWrapper(TerminalWrapper terminal, CommandFactory commandFactory, ManagerWrapper managerWrapper)
        {
            Terminal = terminal;
            CommandFactory = commandFactory;
            Managers = managerWrapper;
        }

        protected List<Context> getRealContexts(List<Context> contexts, RunContext runContext)
        {
            return contexts.Select(context =>
            {
                if (context.Stdin == Context.stdinType.NotUse &&
                    string.IsNullOrEmpty(runContext.File) &&
                    context.Specification.Schema.AcceptMask == false)
                {
                    return context.Specification.Schema.Extensions.Select(x =>
                    {
                        return Directory.GetFiles(context.WorkingDirectory, x, SearchOption.AllDirectories).ToList();
                    }).SelectMany(x => x).ToList()
                    .Select(x =>
                    {
                        var lo = new EngineOptions();
                        context.RunOptions.Select(y =>
                        {
                            if (y.Key == "{path}")
                            {
                                return new KeyValuePair<string, string>(y.Key, Path.GetFileName(x)
                                    .Replace(Path.GetFullPath(context.WorkingDirectory), string.Empty)
                                    .TrimStart('/')
                                    .TrimStart('\\'));
                            }
                            return y;
                        }).ToList().ForEach(z => lo.Add(z.Key, z.Value));

                        return new Context()
                        {
                            ConfigOptions = context.ConfigOptions,
                            Stdin = context.Stdin,
                            WorkingDirectory = Path.GetDirectoryName(x),
                            Specification = context.Specification,
                            RunOptions = lo,
                            Project = context.Project
                        };
                    }).ToList();

                }
                else
                {
                    return new List<Context>()
                    {
                        context
                    };
                }
            }).SelectMany(x => x).ToList();
        } 

        public LinterhubOutputSchema RunAnalyze(
            List<Context> contexts,
            RunContext runContext,
            LinterhubConfigSchema config = null)
        {
            var output = new LinterhubOutputSchema();

            string stdin = null;

            if (contexts.Any(x => x.Stdin != Context.stdinType.NotUse))
            {
                stdin = new StreamReader(runContext.InputStream).ReadToEnd();
            }

            Parallel.ForEach(getRealContexts(contexts, runContext), context =>
            {
                var error = new LinterhubErrorSchema()
                {
                    Code = 0
                };

                var analyzeResult = new EngineOutputSchema();

                try
                {
                    analyzeResult = RunSingleEngine(context, stdin);
                }
                catch (EngineException e)
                {
                    error.Code = (int)e.exitCode;
                    error.Title = e.Title;
                }

                lock (output)
                {
                    var result = new EngineOutputSchema();
                    if (error.Code == 0)
                    {
                        foreach (var current in analyzeResult)
                        {
                            var directoryPrefix = Path.GetFullPath(context.WorkingDirectory).Replace(Path.GetFullPath(context.Project), string.Empty)
                                    .TrimStart('/')
                                    .TrimStart('\\')
                                    .Replace("/", "\\");

                            if (!current.Path.StartsWith(directoryPrefix) && current.Path != string.Empty)
                            {
                                current.Path = Path.Combine(directoryPrefix, current.Path);
                            }

                            current.Path = current
                                 .Path
                                 .Replace(context.Project, string.Empty)
                                 .Replace(Path.GetFullPath(context.Project), string.Empty)
                                 .TrimStart('/')
                                 .TrimStart('\\')
                                 .Replace("/", "\\");
                        }
                        result.AddRange(analyzeResult.Where(x => x.Path != string.Empty));
                    }

                    var exists = output.Find(x => x.Engine == context.Specification.Schema.Name);
                    if (exists != null)
                    {
                        if (exists.Error == null && error.Code == 0)
                        {
                            exists.Result.AddRange(result);
                        }
                        else if (error.Code != 0)
                        {
                            exists.Error = error;
                        }

                    }
                    else
                    {
                        output.Add(new Result()
                        {
                            Result = result,
                            Engine = context.Specification.Schema.Name,
                            Error = error.Code == 0 ? null : error
                        });
                    }

                }

            });

            // Sort and ignore rules
            output.ForEach(x =>
            {
                x.Result.ForEach(y =>
                {
                    y.Messages = y.Messages.Where(m =>
                    {
                        var ignores = new List<LinterhubConfigSchema.IgnoreType>();
                        if (config != null)
                        {
                            if (config.Engines.Where(z => z.Name == x.Engine).Count() > 0)
                            {
                                ignores.AddRange(config.Engines.Find(z => z.Name == x.Engine).Ignore);
                            }
                            ignores.AddRange(config.Ignore);
                            ignores = ignores.Select(r =>
                            {
                                return new LinterhubConfigSchema.IgnoreType()
                                {
                                    Mask = r.Mask == null ? y.Path : (y.Path.Contains(r.Mask) ? y.Path : r.Mask),
                                    RuleId = r.RuleId ?? m.RuleId,
                                    Line = r.Line ?? m.Line
                                };
                            }).ToList();
                        }
                        return !ignores.Exists(r =>
                            r.RuleId == m.RuleId &&
                            r.Line == m.Line &&
                            r.Mask == y.Path
                        );
                    }).OrderBy(z => z.Line).ThenBy(z => z.Column).ThenBy(z => z.RuleId).ToList();
                });
                x.Result.Sort((a, b) => a.Path.CompareTo(b.Path));
            });
            output.Sort((a, b) => a.Engine.CompareTo(b.Engine));

            return output;

        }

        /// <summary>
        /// Run analysis command.
        /// </summary>
        /// <param name="context">The engine context.</param>
        /// <returns>The result.</returns>
        public EngineOutputSchema RunSingleEngine(Context context, string stdin = "")
        {
            var engineName = context.Specification.Schema.Name;
            var tempFile = "#tempfile";

            var workingDirectoryCached = context.WorkingDirectory;

            if (context.Stdin == Context.stdinType.Use)
            {
                tempFile = Path.GetTempFileName();
                File.WriteAllText(tempFile, stdin);
                context.RunOptions.Remove("{path}");
                context.RunOptions.Add("{path}", Path.GetFileName(tempFile));
                context.WorkingDirectory = Path.GetDirectoryName(tempFile);
            }

            var command = CommandFactory.GetAnalyzeCommand(context, context.Locally ? Managers.get(engineName).LocallyExecution(engineName) : null);

            var result = RunEngine(context, command, successCode: context.Specification.Schema.SuccessCode ?? 0, stdin: context.Stdin == Context.stdinType.UseWithEngine ? stdin : string.Empty)
                    .DeserializeAsJson<EngineOutputSchema>();

            result.ForEach((file) => {
                if (context.Stdin != Context.stdinType.NotUse)
                {
                    file.Path = context.RunOptions.Where(x => x.Key == "file://{stdin}").First().Value;
                }
            });

            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }

            context.WorkingDirectory = workingDirectoryCached;

            return result;
        }

        /// <summary>
        /// Run engine command.
        /// </summary>
        /// <param name="context">The engine context.</param>
        /// <param name="command">The command.</param>
        /// <param name="commandSeparator">The command separator.</param>
        /// <param name="successCode">The expected success code.</param>
        /// <returns>The result.</returns>
        protected string RunEngine(Context context, string command, string commandSeparator = " ", int successCode = 0, string stdin = "")
        {
            var result = Terminal.RunTerminal(command, Path.GetFullPath(context.WorkingDirectory), stdin: stdin);

            if (result.RunException != null)
            {
                throw new EngineException("Running engine exception", result.RunException.Message);
            }

            var stdError = result.Error.ToString().Trim();
            var stdOut = result.Output.ToString().Trim();

            if (!string.IsNullOrEmpty(stdError) && result.ExitCode != successCode)
            {
                throw new EngineException(stdError);
            }

            if (result.ExitCode != successCode)
            {
                throw new EngineException($"Unexpected exit code: {result.ExitCode}", string.IsNullOrEmpty(stdOut) ? stdError : stdOut);
            }

            return string.IsNullOrEmpty(stdOut) ? stdError : stdOut;
        }

        public class RunContext
        {
            public string Project { get; set; }
            public string Directory { get; set; }
            public string File { get; set; }
            public Stream InputStream;
        }

        /// <summary>
        /// The engine context.
        /// </summary>
        public class Context
        {
            /// <summary>
            /// Gets or sets the engine specification.
            /// </summary>
            public EngineSpecification Specification { get; set; }

            /// <summary>
            /// Gets or sets run options.
            /// </summary>
            public EngineOptions RunOptions { get; set; }

            /// <summary>
            /// Gets or sets configuration options.
            /// </summary>
            public EngineOptions ConfigOptions { get; set; }

            /// <summary>
            /// Gets or sets the working directory.
            /// </summary>
            public string WorkingDirectory { get; set; }

            public string Project { get; set; }
            public stdinType Stdin { get; set; } = 0;

            public bool Locally { get; set; } = true;

            public enum stdinType
            {
                NotUse,
                Use,
                UseWithEngine
            }
        }

    }
}