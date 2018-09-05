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

        private const string CACHENAME = "LRF_MEMORY_CACHE_NAME";

        private static volatile HashSet<string> _keys;

        private static volatile MemoryCache _objectCache;

        private static object _syncRoot = new object();

        private static HashSet<string> Keys
        {
            get
            {
                if (_keys == null)
                {
                    lock (_syncRoot)
                    {
                        if (_keys == null)
                        {
                            _keys = new HashSet<string>();
                        }
                    }
                }

                return _keys;
            }
        }

        private static MemoryCache CacheInstance
        {
            get
            {
                if (_objectCache == null)
                {
                    lock (_syncRoot)
                    {
                        if (_objectCache == null)
                        {
                            _objectCache = new MemoryCache(CACHENAME);
                        }
                    }
                }

                return _objectCache;
            }
        }

        public bool Contains(string key)
        {
            return CacheInstance.Contains(key);
        }

        public T Get<T>(string key)
        {
            var data = Contains(key) ? (T)CacheInstance[key] : default(T);
            return DeepClone(data);
        }

        public void Set<T>(string key, T value, int seconds = TIMEOUT)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key), "Key should not be empty!");
            }

            CacheInstance.Set(key, DeepClone(value), DateTime.Now.AddSeconds(seconds));

            lock (_syncRoot)
            {
                Keys.Add(key);
            }
        }

        public void Clear()
        {
            while (Keys.Count > 0)
            {
                Remove(Keys.First());
            }
        }

        public void Remove(string key)
        {
            Keys.Remove(key);
            CacheInstance.Remove(key);
        }

        private static T DeepClone<T>(T source)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source));
        }
    }
}