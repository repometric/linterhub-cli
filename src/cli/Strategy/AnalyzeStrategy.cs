namespace Linterhub.Cli.Strategy
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Linq;
    using Runtime;
    using Engine;
    using Engine.Exceptions;
    using System.Threading.Tasks;

    public class AnalyzeStrategy : IStrategy
    {
        public object Run(RunContext context, LinterFactory factory, LogManager log)
        {
            var linterResults = new List<RunResult>();
            try
            {
                Stream input;

                if (!context.InputAwailable)
                {
                    var configs = GetConfigs(context, factory, log);
                    var linterModels = new List<LinterResult>();
                    linterModels.AddRange(
                        configs
                            .Linters.Where(x => x.Command != null)
                            .Select(linterConfig => new LinterResult
                            {
                                Name = linterConfig.Name,
                                Task = Task<string>.Factory.StartNew(() =>
                                    new LinterhubWrapper(context).Analyze(linterConfig.Name, linterConfig.Command, context.Project)
                                   )
                            }
                            ));

                    Task.WaitAll(linterModels.Select(x => x.Task).ToArray());
                    foreach (var linter in linterModels)
                    {
                        using (input = linter.Task.Result.GetMemoryStream())
                        {
                            linterResults.Add(new RunResult
                            {
                                Name = linter.Name,
                                Model = factory.CreateModel(linter.Name, input, null)
                               
                            });
                        }
                    }
                }
                else
                {
                    input = context.Input;
                    linterResults.Add(new RunResult
                    {
                        Name = context.Linter,
                        Model = factory.CreateModel(context.Linter, input, null)
                    });
                }
            }
            catch (Exception exception)
            {
                throw new LinterEngineException(exception.Message, exception);
            }
            return linterResults;
        }

        private ProjectConfig GetConfigs(RunContext context, LinterFactory factory, LogManager log)
        {
            var validateResult = context.ValidateContext(factory, log);
            return validateResult.IsLinterSpecified
                    ? GetLinter(factory, context.Linter, validateResult) :
                        GetLinters(factory, validateResult);
        }

        public ProjectConfig GetLinters(LinterFactory factory, ValidationContext validateContext)
        {
            foreach (var linter in validateContext.ProjectConfig.Linters.Where(x => x.Active != false))
            {
                linter.Command = GetCommand(factory, validateContext, linter);
            }
            return validateContext.ProjectConfig;
        }

        public ProjectConfig GetLinter(LinterFactory factory, string name, ValidationContext validateContext)
        {
            var linter = validateContext.ProjectConfig.Linters.FirstOrDefault(x => x.Name == name && x.Active != false);
            var command = GetCommand(factory, validateContext, linter, name);
            if (linter == null)
            {
                validateContext.ProjectConfig.Linters.Add(new ProjectConfig.Linter
                {
                    Command = command,
                    Name = name,
                });
            }
            else
            {
                linter.Command = command;
            }
            return validateContext.ProjectConfig;
        }

        public string GetCommand(LinterFactory factory, ValidationContext validateConext, ProjectConfig.Linter linter = null, string linterName = null)
        {
            ILinterArgs args = null;
            linter = linter ?? new ProjectConfig.Linter();

            return linter.Command ?? (
                        linter.Active != false ? 
                            factory.BuildCommand(
                                linterName ?? linter.Name,
                                validateConext.WorkDir,
                                validateConext.Path,
                                validateConext.ArgMode,
                                args) : null);
        }
    }
}