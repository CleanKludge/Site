using Newtonsoft.Json;

namespace CleanKludge.Data.File.Serializers
{
    public class JsonSerializer : ISerializer
    {
        public T Deserialize<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }

        public string Serialize<T>(T data)
        {
            return JsonConvert.SerializeObject(data);
        }
    }
}