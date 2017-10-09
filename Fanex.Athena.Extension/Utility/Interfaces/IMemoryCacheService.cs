namespace Fanex.Athena.Extension.Utility.Interfaces
{
    public interface IMemoryCacheService
    {
        void Clear();

        bool Contains(string key);

        void Delete(string key);

        T Get<T>(string key);

        void Set<T>(string key, T value, int seconds = 900);
    }
}