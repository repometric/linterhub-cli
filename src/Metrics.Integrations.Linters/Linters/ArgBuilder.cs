namespace Metrics.Integrations.Linters
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Extensions;
    using System.Collections;

    public class ArgBuilder
    {
        protected int IntDefault { get; }
        protected bool BoolDefault { get; }
        protected object ObjectDefault { get; }
        protected string StringDefault { get; }
        protected List<string> StringListDefault { get; }

        public ArgBuilder(
            int intDefault = default(int),
            bool boolDefault = default(bool),
            string stringDefault = default(string),
            object objectDefault = default(object),
            List<string> stringListDefault = default(List<string>))
        {
            IntDefault = intDefault;
            BoolDefault = boolDefault;
            ObjectDefault = objectDefault;
            StringDefault = stringDefault;
            StringListDefault = stringListDefault;
        }

        public string Build<T>(T configuration)
        {
            var provider = configuration as IArgProvider;
            if (provider != null)
            {
                return provider.Build();
            }

            var properties = GetProperties(configuration);
            var values = properties.Where(x => IsInclude(x.Value));
            return string.Join(" ", properties.Select(BuildArgument));
        }

        private bool IsInclude(object value)
        {
            var isInclude = false;
            TypeSwitch.On(value)
                .Case<int>(v => isInclude = v != IntDefault)
                .Case<bool>(v => isInclude = v != BoolDefault)
                .Case<string>(v => isInclude = !string.IsNullOrEmpty(v) && v != StringDefault)
                .Case<object>(v => isInclude = v != ObjectDefault)
                .Case<List<string>>(v => isInclude = v != StringListDefault && v.Count != 0)
                .Default(v => isInclude = false);

            return isInclude;
        }

        private static string BuildArgument(KeyValuePair<ArgAttribute, object> arg)
        {
            // check if value is List<string>
            if (arg.Value != null && arg.Value is IList)
            {
                var l = (List<string>)arg.Value;
                string res = "";
                l.ForEach(x => {
                    res += " " + BuildArgument(new KeyValuePair<ArgAttribute, object>(arg.Key, x));
                });
                return res;
            }
            const string template = "{0}{1}{2}";
            return string.Format(template, 
                arg.Key.Name,
                arg.Key.Add ? arg.Key.Separator : null,
                arg.Key.Add ? arg.Value : null);
        }

        private static IDictionary<ArgAttribute, object> GetProperties<T>(T configuration)
        {
            return configuration
                .GetType()
                .GetProperties()
                .Select(x => new { Arg = x.GetCustomAttribute<ArgAttribute>(), Value = x.GetValue(configuration) })
                .Where(x => x.Arg != null)
                .OrderBy(x => x.Arg.Order)
                .ToDictionary(x => x.Arg, y => y.Value);
        }
    }
}
