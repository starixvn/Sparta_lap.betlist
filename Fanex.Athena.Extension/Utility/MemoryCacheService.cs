namespace Fanex.Athena.Extension.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Caching;
    using Interfaces;
    using Newtonsoft.Json;

    public class MemoryCacheService : IMemoryCacheService
    {
        private const int TIMEOUT = 900; // 15 minutes.
        private const string REGIOMNAME = "LAP_MAIN_MEMORY";
        private static ObjectCache _instance = new CustomMemoryCache();

        public bool Contains(string key)
        {
            return _instance.Any(i => i.Key == key);
        }

        public T Get<T>(string key)
        {
            var data = Contains(key) ? (T)_instance.Get(key, REGIOMNAME) : default(T);
            return DeepClone(data);
        }

        public void Set<T>(string key, T value, int seconds = TIMEOUT)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key), "Key should not be empty!");
            }

            _instance.Set(key, DeepClone(value), DateTime.Now.AddSeconds(seconds), REGIOMNAME);
        }

        public void Clear()
        {
            var keys = _instance.Where(i => i.Key.StartsWith($"region:{REGIOMNAME}"))
                .Select(i => i.Key).ToList();
            foreach (var key in keys)
            {
                _instance.Remove(key);
            }
        }

        private static T DeepClone<T>(T source)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source));
        }

    }
}