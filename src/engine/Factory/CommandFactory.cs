namespace Linterhub.Engine.Schema
{
    using System.Collections.Generic;
    using System.Linq;
    using Extensions;
    using Option = System.Collections.Generic.KeyValuePair<string, string>;
    using static Runtime.LinterWrapper;

    public class CommandFactory
    {
        public string GetAnalyzeCommand(
            Context context,
            string argSeparator = " ")
        {
            var valueSeparator = context.Specification.Schema.OptionsDelimiter ?? " ";
            var options = MergeOptions(context.ConfigOptions, context.Specification);
            var args = options.Select(x => BuildArg(context.RunOptions, x, valueSeparator, context.Stdin)).Where(x => !string.IsNullOrEmpty(x));
            var command = context.Specification.Schema.Name + " " + string.Join(argSeparator, args);

            if(context.Stdin == Context.stdinType.UseWithLinter)
            {
                command = string.Join(" ", command, BuildArgValue(context.RunOptions, context.Specification.Schema.Stdin.CustomCommand));
            }

            if (context.Specification.Schema.Postfix != null || context.Specification.Schema.Prefix != null)
            {
                var postfix = context.Specification.Schema.Postfix ?? "";
                var prefix = context.Stdin != Context.stdinType.UseWithLinter ? context.Specification.Schema.Prefix ?? "" : "";
                foreach (var runtimeOption in context.RunOptions)
                {
                    postfix = postfix?.Replace(runtimeOption.Key, runtimeOption.Value);
                    prefix = prefix?.Replace(runtimeOption.Key, runtimeOption.Value);
                }
                command = string.Join(" ", prefix, command, postfix);
            }

            return command;
        }

        public string GetVersionCommand(LinterSpecification specification)
        {
            return "--version";
        }

        private List<Option> MergeOptions(
            LinterOptions configOptions,
            LinterSpecification specification)
        {
            var sortedOptions = new List<Option>();
            foreach (var option in specification.OptionsSchema)
            {
                var key = option.Key;
                string value;
                if (option.Value.Id != null)
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
            LinterOptions runtimeOptions,
            Option option,
            string valueSeparator = " ", Context.stdinType stdin = Context.stdinType.NotUse)
        {
            if(stdin == Context.stdinType.UseWithLinter && (option.Value ?? "").Contains("{path}"))
            {
                return "";
            }

            var value = BuildArgValue(runtimeOptions, option.Value);
            var key = option.Key;
            var parts = new [] { key, value }.Where(x => !string.IsNullOrEmpty(x));
            return string.Join(valueSeparator, parts);
        }

        private string BuildArgValue(
            LinterOptions runtimeOptions,
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