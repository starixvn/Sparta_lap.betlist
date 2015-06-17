// -----------------------------------------------------------------------
// <copyright file="CacheService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Caching;
    using Newtonsoft.Json;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class CacheService
    {
        private const int TIMEOUT = 900; // 15 minutes.

        private static readonly ICache Cache = Caching.CacheService.CreateInstance(CacheType.MemoryCache);

        public static T Get<T>(string key)
        {
            return DeepClone(Cache.Get<T>(key));
        }

        public static void Set<T>(string key, T value)
        {
            Cache.Set(key, DeepClone(value), TIMEOUT);
        }

        public static void Delete()
        {
            Cache.Clear();
        }

        public static void Delete(string key)
        {
            Cache.Remove(key);
        }

        private static T DeepClone<T>(T source)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source));
        }
    }
}
