namespace CleanKludge.Data.File.Serializers
{
    public interface ISerializer
    {
        T Deserialize<T>(string data);
        string Serialize<T>(T data);
    }
}