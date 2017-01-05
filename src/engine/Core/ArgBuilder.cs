namespace Linterhub.Engine
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
        protected IEnumerable<string> StringListDefault { get; }

        public ArgBuilder(
            int intDefault = default(int),
            bool boolDefault = default(bool),
            string stringDefault = default(string),
            object objectDefault = default(object),
            IEnumerable<string> stringListDefault = default(IEnumerable<string>))
        {
            IntDefault = intDefault;
            BoolDefault = boolDefault;
            ObjectDefault = objectDefault;
            StringDefault = stringDefault;
            StringListDefault = stringListDefault;
        }

        public string Build<T>(T configuration, string workDir, string path, ArgMode mode)
        {
            var provider = configuration as IArgProvider;
            if (provider != null)
            {
                return provider.Build();
            }

            var properties = GetProperties(configuration);
            var values = properties.Where(x => IsInclude(x.Value) || x.Key is ArgPathAttribute);
            return string.Join(" ", values.Select(x => BuildArgument(x, workDir, path, mode)));
        }

        public string BuildVersion<T>(T configuration)
        {
            var attribute = configuration
                .GetType()
                .GetTypeInfo()
                .GetCustomAttribute<ArgVersionAttribute>();
            
            return attribute != null ? attribute.Name : null;
        }

        private bool IsInclude(object value)
        {
            var isInclude = false;
            TypeSwitch.On(value)
                .Case<int>(v => isInclude = v != IntDefault)
                .Case<bool>(v => isInclude = v != BoolDefault)
                .Case<string>(v => isInclude = !string.IsNullOrEmpty(v) && v != StringDefault)
                .Case<object>(v => isInclude = v != ObjectDefault)
                .Case<IEnumerable<string>>(v => isInclude = !Equals(v, StringListDefault) && v.Count() != 0)
                .Default(v => isInclude = false);

            return isInclude;
        }

        private static string BuildArgument(KeyValuePair<ArgAttribute, object> arg, string workDir, string path, ArgMode mode)
        {
            if (arg.Value is IEnumerable<string>)
            {
                var r = ((IEnumerable<string>)arg.Value).Select(z => new KeyValuePair<ArgAttribute, object>(arg.Key, z));
                return string.Join(" ", r.Select(x => BuildArgument(x, workDir, path, mode)));
            }
            var isPath = arg.Key is ArgPathAttribute && !string.IsNullOrEmpty(path);
            const string template = "{0}{1}{2}";
            return string.Format(template, 
                arg.Key.Name,
                arg.Key.Add ? arg.Key.Separator : null,
                arg.Key.Add ? isPath ? ((ArgPathAttribute)arg.Key).GetPath(workDir, path, mode) : arg.Value : null);
        }

        private static IDictionary<ArgAttribute, object> GetProperties<T>(T configuration)
        {
            var properties = configuration.GetType().GetProperties();
            var targetType = typeof(ArgAttribute);
            var query =
                from property in properties
                let attributes = property.GetCustomAttributes()
                let attribute = attributes.FirstOrDefault(x => targetType.IsInstanceOfType(x))
                let arg = attribute as ArgAttribute
                where arg != null
                orderby arg.Order
                select new
                {
                    Arg = arg,
                    Value = property.GetValue(configuration)
                };

            return query.ToDictionary(x => x.Arg, x => x.Value);
        }
    }
}
