namespace Linterhub.Engine.Schema
{
    using System.Collections.Generic;
    using System.Linq;
    using Linterhub.Engine.Extensions;
    using Option = System.Collections.Generic.KeyValuePair<string, string>;
    using System;

    public class CommandFactory
    {
        public string GetAnalyzeCommand(
            LinterSpecification specification,
            LinterOptions runtimeOptions, 
            LinterOptions configOptions,
            string argSeparator = " ")
        {
            var valueSeparator = specification.Schema.OptionsDelimiter ?? " ";
            var options = MergeOptions(configOptions, specification);
            var args = options.Select(x => BuildArg(runtimeOptions, x, valueSeparator)).Where(x => !string.IsNullOrEmpty(x));
            var command = specification.Schema.Name + " " + string.Join(argSeparator, args);
            if (specification.Schema.Postfix != null || specification.Schema.Prefix != null)
            {
                var postfix = specification.Schema.Postfix ?? "";
                var prefix = specification.Schema.Prefix ?? "";
                foreach (var runtimeOption in runtimeOptions)
                {
                    postfix = postfix?.Replace(runtimeOption.Key, runtimeOption.Value);
                    prefix = prefix?.Replace(runtimeOption.Key, runtimeOption.Value);
                }
                command = prefix + command + postfix;
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
            string valueSeparator = " ")
        {
            var value = option.Value;
            var key = option.Key;
            foreach (var runtimeOption in runtimeOptions)
            {
                value = value?.Replace(runtimeOption.Key, runtimeOption.Value);
            }
            var parts = new [] { key, value }.Where(x => !string.IsNullOrEmpty(x));
            return string.Join(valueSeparator, parts);
        }
    }
}