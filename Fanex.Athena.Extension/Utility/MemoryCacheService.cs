namespace Fanex.Athena.Extension.Utility
{
    using Fanex.Caching;
    using Interfaces;
    using Newtonsoft.Json;

    public class MemoryCacheService : IMemoryCacheService
    {
        private const int TIMEOUT = 900; // 15 minutes.

        private readonly ICache _cache;

        public MemoryCacheService()
        {
            _cache = CacheService.CreateInstance(CacheType.MemoryCache);
        }

        public bool Contains(string key)
        {
            return _cache.Contains(key);
        }

        public T Get<T>(string key)
        {
            return DeepClone(_cache.Get<T>(key));
        }

        public void Set<T>(string key, T value, int seconds = TIMEOUT)
        {
            _cache.Set(key, DeepClone(value), seconds);
        }

        public void Clear()
        {
            _cache.Clear();
        }

        public void Delete(string key)
        {
            _cache.Remove(key);
        }

        private static T DeepClone<T>(T source)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source));
        }
    }
}