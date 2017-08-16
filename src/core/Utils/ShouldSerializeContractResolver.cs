namespace Linterhub.Core.Utils
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// Represents custom contract resolver with ability to skip some properties during json serialization.
    /// </summary>
    public class ShouldSerializeContractResolver : DefaultContractResolver
    {
        /// <summary>
        /// Gets the list of properties to serialize.
        /// </summary>
        protected IEnumerable<string> Keys { get; }

        /// <summary>
        /// Initializes a new instance of <seealso cref="ShouldSerializeContractResolver"/> class.
        /// </summary>
        /// <param name="keys">The list of allowed properties.</param>
        public ShouldSerializeContractResolver(IEnumerable<string> keys = null)
        {
            Keys = keys?.Select(x => x.ToLower()) ?? new List<string>();
        }

        protected override System.Collections.Generic.IList<JsonProperty> CreateProperties(System.Type type, MemberSerialization memberSerialization)
        {
            return base.CreateProperties(type, memberSerialization).OrderBy(p => p.PropertyName).ToList();
        }

        /// <summary>
        /// Creates a <seealso cref="JsonProperty"/> for the given <seealso cref="MemberInfo"/>.
        /// </summary>
        /// <param name="member">The member to create a <seealso cref="JsonProperty"/> for.</param>
        /// <param name="memberSerialization">The member's parent <seealso cref="MemberSerialization"/>.</param>
        /// <returns>A created <seealso cref="JsonProperty"/> for the given <seealso cref="MemberInfo"/>.</returns>
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            var name = property.PropertyName.ToLower();
            var isDefaultValueIgnored = ((property.DefaultValueHandling ?? DefaultValueHandling.Ignore) & DefaultValueHandling.Ignore) != 0;
            var isCollection = !typeof(string).IsAssignableFrom(property.PropertyType) && typeof(IEnumerable).IsAssignableFrom(property.PropertyType);

            // Ignore not listed keys, default values and empty collections
            var nullPredicate = (Predicate<object>)null;
            var predicates = new List<Predicate<object>>
            {
                Keys.Any() && !Keys.Contains(name) ? instance => false : nullPredicate,
                isDefaultValueIgnored && isCollection ? instance =>
                {
                    var collection = property.ValueProvider.GetValue(instance) as IEnumerable;
                    return collection != null && collection.GetEnumerator().MoveNext();
                } : nullPredicate
            };

            property.ShouldSerialize = instance => predicates.Where(x => x != null).All(x => x(instance));
            return property;
        }

        /// <summary>
        /// Resolves the name of the property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>Resolved name of the property.</returns>
	    protected override string ResolvePropertyName(string propertyName)
        {
            return ToCamelCase(propertyName);
        }

        /// <summary>
        /// Convert string to camel case.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <returns>The camel case string value.</returns>
        private string ToCamelCase(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            return value.Substring(0, 1).ToLower() +
                   value.Substring(1);
        }
    }
}