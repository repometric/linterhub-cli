namespace Linterhub.Core.Factory
{
    using System.Collections.Generic;
    using System.Linq;
    using Utils;
    using Option = System.Collections.Generic.KeyValuePair<string, string>;
    using static Runtime.EngineWrapper;
    using Schema;
    using System.IO;

    public class CommandFactory
    {
        public string GetAnalyzeCommand(
            EngineContext context,
            string executePath = null,
            string argSeparator = " ")
        {
            var valueSeparator = context.Specification.Schema.OptionsDelimiter ?? " ";
            var options = MergeOptions(context.ConfigOptions, context.Specification);

            if (context.Stdin == EngineContext.stdinType.UseWithEngine)
            {
                options.AddRange(context.Specification.Schema.Stdin);
            }

            var args = options.Select(x => BuildArg(context.RunOptions, x, valueSeparator, context.Stdin)).Where(x => !string.IsNullOrEmpty(x));

            if (executePath != null)
            {
                executePath = Path.GetFullPath(Path.Combine(context.Project, executePath).NormalizePath());
            }

            var command = (executePath ?? 
                          context.Specification.Schema.Executable ?? 
                          context.Specification.Schema.Name) + " " + string.Join(argSeparator, args);

            if (context.Specification.Schema.Postfix != null)
            {
                var postfix = context.Specification.Schema.Postfix ?? "";
                postfix = BuildArgValue(context.RunOptions, postfix);
                command = string.Join(" ", command, postfix);
            }

            return command;
        }

        private List<Option> MergeOptions(
            EngineOptions configOptions,
            EngineSpecification specification)
        {
            var sortedOptions = new List<Option>();
            foreach (var option in specification.OptionsSchema)
            {
                var key = option.Key;
                string value;
                if (option.Value.Id == "{path}")
                {
                    value = option.Value.Id;
                }
                else
                {
                    value = configOptions.GetValueOrDefault(key) ??
                            specification.Schema.Defaults.GetValueOrDefault(key);
                }

                if (value != null || (option.Value.Type == "null" && specification.Schema.Defaults.ContainsKey(key)))
                {
                    sortedOptions.Add(new Option(key, value));
                }
            }

            return sortedOptions;
        }

        private string BuildArg(
            EngineOptions runtimeOptions,
            Option option,
            string valueSeparator = " ", EngineContext.stdinType stdin = EngineContext.stdinType.NotUse)
        {
            if(stdin == EngineContext.stdinType.UseWithEngine && (option.Value ?? "").Contains("{path}"))
            {
                return "";
            }

            var value = BuildArgValue(runtimeOptions, option.Value);
            var key = option.Key;
            var parts = new [] { key, value }.Where(x => !string.IsNullOrEmpty(x));
            return string.Join(valueSeparator, parts);
        }

        private string BuildArgValue(
            EngineOptions runtimeOptions,
            string value)
        {
            foreach (var runtimeOption in runtimeOptions)
            {
                value = value?.Replace(runtimeOption.Key, runtimeOption.Value);
            }
            return value;
        }
    }
}