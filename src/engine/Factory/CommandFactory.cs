namespace Linterhub.Engine.Schema
{
    using System.Collections.Generic;
    using System.Linq;
    using Linterhub.Engine.Extensions;
    using Option = System.Collections.Generic.KeyValuePair<string, string>;

    public class CommandFactory
    {
        public string GetAnalyzeCommand(
            LinterSpecification specification,
            LinterOptions runtimeOptions, 
            LinterOptions configOptions,
            string argSeparator = " ")
        {
            var options = MergeOptions(configOptions, specification);
            var args = options.Select(x => BuildArg(runtimeOptions, x)).Where(x => !string.IsNullOrEmpty(x));
            var command = string.Join(argSeparator, args);
            if (specification.Schema.Postfix != null)
            {
                command = command + specification.Schema.Postfix;
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