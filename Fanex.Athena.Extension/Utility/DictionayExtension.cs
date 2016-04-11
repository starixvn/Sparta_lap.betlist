using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Athena.Extension.Utility
{
    public static class DictionayExtension
    {
        public static TValue GetValueOrDefault<TKey, TValue>
     (this IDictionary<TKey, TValue> dictionary,
      TKey key,
      TValue defaultValue)
        {
            TValue value;
            return dictionary.TryGetValue(key, out value) ? value : defaultValue;
        }

        public static TValue GetValueOrDefault<TKey, TValue>
            (this IDictionary<TKey, TValue> dictionary,
             TKey key,
             Func<TValue> defaultValueProvider)
        {
            return dictionary.GetValueOrDefault(key, defaultValueProvider());
        }
    }
}
