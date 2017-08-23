namespace Linterhub.Core.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents custom contract resolver with ability to skip some objects during json serialization.
    /// </summary>
    public class ShouldSerializeConverter : JsonConverter
    {
        /// <summary>
        /// Gets the list of custom filters using during the serialization.
        /// </summary>
        protected IEnumerable<Predicate<object>> Filters { get; }

        /// <summary>
        /// Gets the list of supported types.
        /// </summary>
        protected IEnumerable<Type> Types { get; }

        /// <summary>
        /// Initializes a new instance of <seealso cref="ShouldSerializeConverter"/> class.
        /// </summary>
        /// <param name="supportedCollections">The supported filterable objects.</param>
        /// <param name="filters">The custom value filters.</param>
        public ShouldSerializeConverter(IEnumerable<Type> supportedTypes, IEnumerable<string> filters = null)
        {
            Types = supportedTypes;
            Filters = ParseFilters(filters ?? new List<string>());
        }

        private IEnumerable<Predicate<object>> ParseFilters(IEnumerable<string> filters)
        {
            // Only equeals operator is supported right now
            // TODO: Move to separate class and add more filters
            var result = new List<Predicate<object>>();
            foreach (var filter in filters)
            {
                var parts = filter.Split('=');
                var name = parts[0];
                var value = parts[1];
                Predicate<object> filterPredicate = x => 
                {
                    var propertyValue = x.GetPropertyValue(name);
                    var collection = propertyValue as IEnumerable<object>;
                    return 
                        propertyValue == null ||
                        (collection == null && propertyValue.ToString().ToLower() == value.ToLower()) ||
                        (collection != null && collection.Any(z => z.ToString().ToLower() == value.ToLower()));
                };
                result.Add(filterPredicate);
            }

            return result;
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>true if this instance can convert the specified object type; otherwise, false.</returns>
        public override bool CanConvert(Type objectType)
        {
            return Types.Any(x => x.IsAssignableFrom(objectType));
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <seealso cref="JsonWriter"/> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is IEnumerable<object>)
            {
                // The collection of elements should be serialized one by one in order to avoid circular dependencies.
                var collection = (IEnumerable<object>)value;
                collection = collection.Where(x => Filters.All(y => y(x))).ToArray();
                writer.WriteStartArray();
                foreach (var item in collection)
                {
                    serializer.Serialize(writer, item);
                }
                writer.WriteEndArray();
            }
            else
            {
                if (Filters.All(x => x(value)))
                {
                    serializer.Serialize(writer, value);
                }
            }     
        }

        /// <summary>
        /// Gets a value indicating whether this converter can read.
        /// </summary>
        public override bool CanRead
        {
            get { return false; }
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <seealso cref="JsonReader"/> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}