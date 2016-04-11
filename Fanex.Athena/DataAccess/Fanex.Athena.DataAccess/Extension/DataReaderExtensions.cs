// -----------------------------------------------------------------------
// <copyright file="DataReaderExtensions.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Extension
{
    using System;
    using System.Data;
    using Fanex.Common.Extensions;

    public static class DataReaderExtensions
    {
        public static T GetValueOrDefault<T>(this IDataReader reader, string key, T defaultValue)
        {
            try
            {
                var columnIndex = reader.GetOrdinal(key);
                var result = reader.GetValue(columnIndex);
                if (!object.ReferenceEquals(result, null) && result != DBNull.Value)
                {
                    return (T)Convert.ChangeType(result, typeof(T));
                }

                return defaultValue;
            }
            catch (IndexOutOfRangeException)
            {
            }

            return defaultValue;
        }

        public static T GetValueOrDefault<T>(this IDataReader reader, string key, Func<T> defaultValueProvider)
        {
            return reader.GetValueOrDefault(key, defaultValueProvider());
        }

        public static bool HasColumn(this IDataRecord reader, string columnName)
        {
            return reader.TryGetOrdinal(columnName) >= 0;
        }

        /// <summary>
        /// Return -1 if field does not exist.
        /// </summary>
        /// <param name="reader">Reader value.</param>
        /// <param name="columnName">Field name value.</param>
        /// <returns>The result value.</returns>
        public static int TryGetOrdinal(this IDataRecord reader, string columnName)
        {
            var fieldCount = reader.FieldCount;

            for (var i = 0; i < fieldCount; i++)
            {
                var name = reader.GetName(i);
                if (name.Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }

            return -1;
        }

        public static int? ToNullableInt(this IDataReader reader, string fieldName, int? defaultValue = null)
        {
            return reader[fieldName] != DBNull.Value ? reader.ToInt(fieldName) : defaultValue;
        }

        public static decimal? ToNullableDecimal(this IDataReader reader, string fieldName, decimal? defaultValue = null)
        {
            return reader[fieldName] != DBNull.Value ? reader.ToDecimal(fieldName) : defaultValue;
        }
    }
}