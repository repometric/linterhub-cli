namespace Linterhub.Core.Runtime
{
    using Extensions;
    using Schema;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class EngineRunner
    {
        private EngineWrapper engineRunner;

        public EngineRunner(EngineWrapper wrapper)
        {
            engineRunner = wrapper;
        }

        public List<EngineOutputSchema.ResultType> RunAnalyze(
            List<EngineWrapper.Context> contexts,
            string project,
            string directory,
            string file,
            LinterhubConfigSchema config = null)
        {
            var results = new List<EngineOutputSchema.ResultType>();
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
                    .ForEach(x => {
                        var lo = new EngineOptions();
                        context.RunOptions.Select(y => {
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

                var res = engineRunner.RunAnalysis(context, stdin);
                var current = res.DeserializeAsJson<EngineOutputSchema.ResultType[]>();
                lock (results)
                {
                    foreach (var output in current)
                    {
                        if (!string.IsNullOrEmpty(directory) && output.Path != string.Empty)
                        {
                            var directoryPrefix = Path.GetFullPath(directory).Replace(Path.GetFullPath(project), string.Empty)
                                .TrimStart('/')
                                .TrimStart('\\')
                                .Replace("/", "\\");

                            if (!output.Path.Contains(directoryPrefix))
                            {
                                output.Path = Path.Combine(Path.GetFullPath(directory), output.Path);
                            }
                        }

                        output.Path = output
                             .Path
                             .Replace(project, string.Empty)
                             .Replace(Path.GetFullPath(project), string.Empty)
                             .TrimStart('/')
                             .TrimStart('\\')
                             .Replace("/", "\\");

                        var req = results.Where(x => x.Path == output.Path);
                        if (req.Count() > 0)
                        {
                            req.First().Messages.AddRange(output.Messages);
                            req.First().Messages = req.First().Messages.OrderBy(x => x.Line).ThenBy(x => x.Column).ThenBy(x => x.RuleId).ToList();
                        }
                        else if (output.Path != string.Empty)
                        {
                            results.Add(output);
                        }
                    }
                }

            });

            if(config != null)
            {
                results = results.Select(x =>
                {
                    x.Messages = x.Messages.Where(m =>
                    {
                        return !(config.Ignore.Find(r => x.Path.Contains(r.Mask)) != null || config.Ignore.Find(r => x.Path.Contains(r.Mask) && m.RuleId == r.RuleId) != null ||
                            config.Ignore.Find(r => x.Path.Contains(r.Mask) && m.Line == r.Line) != null);
                    }).ToList();
                    return x;
                }).ToList();
            }

            return results.OrderBy((x) => x.Path).ToList();

        }
    }
}
