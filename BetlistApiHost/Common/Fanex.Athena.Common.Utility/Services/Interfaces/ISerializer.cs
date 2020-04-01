namespace Fanex.Athena.Common.Utility.Services.Interfaces
{
    public interface ISerializer
    {
        string Serialize<T>(T data);

        T Deserialize<T>(string data);
    }
}