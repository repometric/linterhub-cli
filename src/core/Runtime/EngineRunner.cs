namespace Linterhub.Core.Runtime
{
    using Exceptions;
    using Extensions;
    using Schema;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Result = Schema.LinterhubOutputSchema.ResultType;

    public class EngineRunner
    {
        private EngineWrapper engineRunner;

        public EngineRunner(EngineWrapper wrapper)
        {
            engineRunner = wrapper;
        }

        public LinterhubOutputSchema RunAnalyze(
            List<EngineWrapper.Context> contexts,
            string project,
            string directory,
            string file,
            LinterhubConfigSchema config = null)
        {
            var output = new LinterhubOutputSchema();
            var n_contexts = new List<EngineWrapper.Context>();

            string stdin = "";

            if (contexts.Any(x => x.Stdin != EngineWrapper.Context.stdinType.NotUse))
            {
                stdin = new StreamReader(System.Console.OpenStandardInput()).ReadToEnd();
            }

            contexts.ForEach(context =>
            {
                if (context.Stdin == EngineWrapper.Context.stdinType.NotUse &&
                    string.IsNullOrEmpty(file) &&
                    context.Specification.Schema.AcceptMask == false)
                {
                    context.Specification.Schema.Extensions.Select(x =>
                    {
                        return Directory.GetFiles(context.WorkingDirectory, x, System.IO.SearchOption.AllDirectories).ToList();
                    }).SelectMany(x => x).ToList()
                    .ForEach(x =>
                    {
                        var lo = new EngineOptions();
                        context.RunOptions.Select(y =>
                        {
                            if (y.Key == "{path}")
                            {
                                return new KeyValuePair<string, string>(y.Key, Path.GetFullPath(x)
                                    .Replace(Path.GetFullPath(context.WorkingDirectory), string.Empty)
                                    .TrimStart('/')
                                    .TrimStart('\\'));
                            }
                            return y;
                        }).ToList().ForEach(z => lo.Add(z.Key, z.Value));

                        n_contexts.Add(new EngineWrapper.Context()
                        {
                            ConfigOptions = context.ConfigOptions,
                            Stdin = context.Stdin,
                            WorkingDirectory = context.WorkingDirectory,
                            Specification = context.Specification,
                            RunOptions = lo
                        });
                    });

                }
                else
                {
                    n_contexts.Add(context);
                }
            });

            Parallel.ForEach(n_contexts, context =>
            {
                var error = new LinterhubOutputSchema.ErrorType()
                {
                    Code = 0
                };
                EngineOutputSchema parsedOutput = null;
                try
                {
                    var res = engineRunner.RunAnalysis(context, stdin);
                    parsedOutput = res.DeserializeAsJson<EngineOutputSchema>();
                }
                catch (EngineException e)
                {
                    error.Code = (int)e.exitCode;
                    error.Title = e.Message;
                }
                lock (output)
                {
                    var result = new EngineOutputSchema();
                    if (error.Code == 0)
                    {
                        foreach (var current in parsedOutput)
                        {
                            if (!string.IsNullOrEmpty(directory) && current.Path != string.Empty)
                            {
                                var directoryPrefix = Path.GetFullPath(directory).Replace(Path.GetFullPath(project), string.Empty)
                                    .TrimStart('/')
                                    .TrimStart('\\')
                                    .Replace("/", "\\");

                                if (!current.Path.Contains(directoryPrefix))
                                {
                                    current.Path = Path.Combine(Path.GetFullPath(directory), current.Path);
                                }
                            }

                            current.Path = current
                                 .Path
                                 .Replace(project, string.Empty)
                                 .Replace(Path.GetFullPath(project), string.Empty)
                                 .TrimStart('/')
                                 .TrimStart('\\')
                                 .Replace("/", "\\");
                        }
                        result.AddRange(parsedOutput.Where(x => x.Path != string.Empty));
                    }

                    if (result.Count != 0)
                    {
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

                }

            });

            output.ForEach(x =>
            {
                x.Result.ForEach(y =>
                {
                    y.Messages = y.Messages.Where(m =>
                    {
                        return config != null ? !(config.Ignore.Find(r => y.Path.Contains(r.Mask)) != null || config.Ignore.Find(r => y.Path.Contains(r.Mask) && m.RuleId == r.RuleId) != null ||
                            config.Ignore.Find(r => y.Path.Contains(r.Mask) && m.Line == r.Line) != null) : true;
                    }).OrderBy(z => z.Line).ThenBy(z => z.Column).ThenBy(z => z.RuleId).ToList();
                });
                x.Result.Sort((a, b) => a.Path.CompareTo(b.Path));
            });
            output.Sort((a, b) => a.Engine.CompareTo(b.Engine));

            return output;

        }
    }
}
