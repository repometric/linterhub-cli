namespace Linterhub.Engine.Extensions
{
    using System.IO;
    using System;
    using System.Text;
    using Newtonsoft.Json;
    using System.Xml.Serialization;
    using Newtonsoft.Json.Linq;

    public static class Deserialize
    {
        public static T DeserializeAsJson<T>(this Stream self, Type type = null)
        {
            var serializer = new JsonSerializer();
            // TODO: Refactor.
            using (var reader = new StreamReader(self, Encoding.UTF8, true, 4096, true))
            using (var jsonTextReader = new JsonTextReader(reader))
            {
                return type == null
                       ? serializer.Deserialize<T>(jsonTextReader)
                       : (T)serializer.Deserialize(jsonTextReader, type);
            }
        }

        public static dynamic DeserializeDynamic(this Stream self)
        {
            using (var reader = new StreamReader(self, Encoding.UTF8, true, 4096, true))
            using (var jsonTextReader = new JsonTextReader(reader))
            {
                dynamic value = (JObject)JToken.ReadFrom(jsonTextReader);
                return value;
            }
        }

        public static dynamic DeserializeDynamic(this string self)
        {
            return JsonConvert.DeserializeObject(self);
        }



        public static T DeserializeAsJson<T>(this string self)
        {
            return JsonConvert.DeserializeObject<T>(self);
        }

        public static T DeserializeAsXml<T>(this Stream self)
        {
            var deserializer = new XmlSerializer(typeof(T));
            return (T)deserializer.Deserialize(self);
        }

        public static string SerializeAsJson<T>(this T self)
        {
            return JsonConvert.SerializeObject(self, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }
    }
}
