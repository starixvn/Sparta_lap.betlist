namespace Fanex.Athena.Extension.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Caching;
    using Interfaces;
    using Newtonsoft.Json;

    public class MemoryCacheService : MemoryCache, IMemoryCacheService
    {
        private const int TIMEOUT = 900; // 15 minutes.
        private const string REGIOMNAME = "LAP_MAIN_MEMORY";

        public MemoryCacheService() : base("LAP_MEMORY_CUSTOM_CACHING") { }
        public override void Set(CacheItem item, CacheItemPolicy policy)
        {
            Set(item.Key, item.Value, policy, item.RegionName);
        }
        public override void Set(string key, object value, DateTimeOffset absoluteExpiration, string regionName = null)
        {
            Set(key, value, new CacheItemPolicy { AbsoluteExpiration = absoluteExpiration }, regionName);
        }

        public override void Set(string key, object value, CacheItemPolicy policy, string regionName = null)
        {
            base.Set(CreateKeyWithRegion(key, regionName), value, policy);
        }

        public override CacheItem GetCacheItem(string key, string regionName = null)
        {
            CacheItem temporary = base.GetCacheItem(CreateKeyWithRegion(key, regionName));
            return new CacheItem(key, temporary.Value, regionName);
        }

        public override object Get(string key, string regionName = null)
        {
            return base.Get(CreateKeyWithRegion(key, regionName));
        }

        public override DefaultCacheCapabilities DefaultCacheCapabilities
        {
            get
            {
                return (base.DefaultCacheCapabilities | DefaultCacheCapabilities.CacheRegions);
            }
        }

        private static string CreateKeyWithRegion(string key, string regionName)
        {
            return $"region:{regionName ?? "null_region"};key={key}";
        }


        public bool Contains(string key)
        {
            return this.Any(i => i.Key == key);
        }

        public T Get<T>(string key)
        {
            var data = Contains(key) ? (T)this.Get(key, REGIOMNAME) : default(T);
            return DeepClone(data);
        }

        public void Set<T>(string key, T value, int seconds = TIMEOUT)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key), "Key should not be empty!");
            }

            this.Set(key, DeepClone(value), DateTime.Now.AddSeconds(seconds), REGIOMNAME);
        }

        public void Clear()
        {
            var keys = this.Where(i => i.Key.StartsWith($"region:{REGIOMNAME}"))
                .Select(i => i.Key).ToList();
            foreach (var key in keys)
            {
                base.Remove(key);
            }
        }

        private static T DeepClone<T>(T source)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source));
        }

        public void Remove(string key)
        {
            base.Remove(CreateKeyWithRegion(key, REGIOMNAME));
        }
    }
}