namespace Metrics.Integrations.Extensions
{
    using System.IO;
    using Newtonsoft.Json;
    using System.Xml.Serialization;
    
    public static class Deserialize
    {
        public static T DeserializeAsJson<T>(this Stream self)
        {
            var serializer = new JsonSerializer();
            using (var reader = new StreamReader(self))
            using (var jsonTextReader = new JsonTextReader(reader))
            {
                return serializer.Deserialize<T>(jsonTextReader);
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
    }
}
