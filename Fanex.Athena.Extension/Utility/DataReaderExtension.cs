using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Fanex.Athena.Extension.Utility
{
    public static class DataReaderExtension
    {
        public static T GetValueOrDefault<T>
    (this IDataReader reader,
     string key,
     T defaultValue)
        {
            if (reader.GetOrdinal(key) >= 0)
            {
                var result = reader[key];
                return result != null && result != DBNull.Value ? (T)result : defaultValue;
            }

            return defaultValue;

        }

        public static T GetValueOrDefault<T>
            (this IDataReader reader,
              string key,
             Func<T> defaultValueProvider)
        {
            return reader.GetValueOrDefault(key, defaultValueProvider());
        }
    }
}
