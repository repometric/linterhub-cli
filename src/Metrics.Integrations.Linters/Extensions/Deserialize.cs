namespace Metrics.Integrations.Extensions
{
    using System.IO;
    using Newtonsoft.Json;

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
    }
}
