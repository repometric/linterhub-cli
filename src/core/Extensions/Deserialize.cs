namespace Linterhub.Core.Extensions
{
    using System.IO;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using Core.Utils;
    using System.Linq;
    using System.Reflection;
    using Core.Schema;

    public static class Deserialize
    {
        public static object GetPropertyValue(this object obj, string propertyName)
        {
            foreach (var prop in propertyName.Split('.').Select(s => obj?.GetType()?.GetProperty(s, BindingFlags.IgnoreCase |  BindingFlags.Public | BindingFlags.Instance)))
            {
                obj = prop?.GetValue(obj, null);
            }

            return obj;
        }

        public static string NormalizePath(this string self)
        {
            return self?.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
        }

        public static string RemoveNewline(string str)
        {
            var result = str.Replace("\r", "").Replace("\n", "");
            return string.IsNullOrEmpty(result) ? null : result;
        }

        public static TValue GetValueOrDefault<TKey, TValue>
            (this IDictionary<TKey, TValue> dictionary, 
            TKey key,
            TValue defaultValue = default(TValue))
        {
            TValue value;
            return dictionary.TryGetValue(key, out value) ? value : defaultValue;
        }

        public static T DeserializeAsJsonFromFile<T>(this string self)
            where T : new()
        {
            if (!string.IsNullOrEmpty(self) && File.Exists(self))
            {
                return File.ReadAllText(self).DeserializeAsJson<T>();
            }

            return new T();
        }

        public static T DeserializeAsJson<T>(this string self)
        {
            return JsonConvert.DeserializeObject<T>(self);
        }

        public static string SerializeAsJson<T>(this T self, IEnumerable<string> allowedNames = null, IEnumerable<string> filters = null)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new ShouldSerializeContractResolver(allowedNames),
                DefaultValueHandling = DefaultValueHandling.Populate,
            };

            if (filters != null && filters.Any())
            {
                // TODO: Now it supports only engine schema.
                var supportedTypes =  new[] { typeof(IEnumerable<EngineSchema>) };
                settings.Converters.Add(new ShouldSerializeConverter(supportedTypes, filters));
            }

            return JsonConvert.SerializeObject(self, Formatting.Indented, settings);
        }
    }
}
