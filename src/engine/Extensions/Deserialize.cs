namespace Linterhub.Engine.Extensions
{
    using System.IO;
    using System;
    using Newtonsoft.Json;
    using System.Xml.Serialization;
    
    public static class Deserialize
    {
        public static T DeserializeAsJson<T>(this Stream self, Type type = null)
        {
            var serializer = new JsonSerializer();
            using (var reader = new StreamReader(self))
            using (var jsonTextReader = new JsonTextReader(reader))
            {
                return type == null
                       ? serializer.Deserialize<T>(jsonTextReader)
                       : (T)serializer.Deserialize(jsonTextReader, type);
            }
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
