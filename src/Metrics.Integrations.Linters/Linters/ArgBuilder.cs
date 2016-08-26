namespace Metrics.Integrations.Linters
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Extensions;

    public class ArgBuilder
    {
        protected int IntDefault { get; }
        protected bool BoolDefault { get; }
        protected object ObjectDefault { get; }
        protected string StringDefault { get; }

        public ArgBuilder(
            int intDefault = default(int),
            bool boolDefault = default(bool),
            string stringDefault = default(string),
            object objectDefault = default(object))
        {
            IntDefault = intDefault;
            BoolDefault = boolDefault;
            ObjectDefault = objectDefault;
            StringDefault = stringDefault;
        }

        public string Build<T>(T configuration)
        {
            var properties = GetProperties(configuration);
            var values = properties.Where(x => IsInclude(x.Value));
            
            return string.Join(" ", values.Select(BuildArgument));
        }

        private bool IsInclude(object value)
        {
            var isInclude = false;
            TypeSwitch.On(value)
                .Case<int>(v => isInclude = v != IntDefault)
                .Case<bool>(v => isInclude = v != BoolDefault)
                .Case<string>(v => isInclude = !string.IsNullOrEmpty(v) && v != StringDefault)
                .Case<object>(v => isInclude = v != ObjectDefault)
                .Default(v => isInclude = false);

            return isInclude;
        }

        private static string BuildArgument(KeyValuePair<ArgAttribute, object> arg)
        {
            const string template = "{0}{1}{2}";
            return string.Format(template, 
                arg.Key.Name,
                arg.Key.AddValue ? arg.Key.ValueSeparator : null,
                arg.Key.AddValue ? arg.Value : null);
        }

        private static IDictionary<ArgAttribute, object> GetProperties<T>(T configuration)
        {
            return typeof(T)
                .GetTypeInfo()
                .GetProperties()
                .Select(x => new { Arg = x.GetCustomAttribute<ArgAttribute>(), Value = x.GetValue(configuration) })
                .Where(x => x.Arg != null)
                .OrderBy(x => x.Arg.Order)
                .ToDictionary(x => x.Arg, y => y.Value);
        }
    }
}
