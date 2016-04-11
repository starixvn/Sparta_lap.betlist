// -----------------------------------------------------------------------
// <copyright file="DataReaderExtensions.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Extension
{
    using System;
    using System.Data;
    using Fanex.Common.Extensions;
    using Logging;
    /// <summary>
    ///     The data reader extensions.
    /// </summary>
    public static class DataReaderExtensions
    {
        public static T GetValueOrDefault<T>(this IDataReader reader, string key, T defaultValue)
        {
            try
            {
                var columnIndex = reader.GetOrdinal(key);
                if (columnIndex >= 0)
                {
                    var result = reader.GetValue(columnIndex);
                    if (!object.ReferenceEquals(result, null) && result != DBNull.Value)
                    {
                        return (T)((object)Convert.ChangeType(result, typeof(T)));
                    }

                    return defaultValue;
                }
            }
            catch (IndexOutOfRangeException exception)
            {
                Logger.Log.Error(exception.Message, exception);
            }

            return defaultValue;
        }

        public static T GetValueOrDefault<T>(this IDataReader reader, string key, Func<T> defaultValueProvider)
        {
            return reader.GetValueOrDefault(key, defaultValueProvider());
        }

        public static bool FieldExists(this IDataReader reader, string fieldName)
        {
            reader.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", fieldName);
            return reader.GetSchemaTable().DefaultView.Count > 0;
        }

        /// <summary>
        /// Return -1 if field does not exist.
        /// </summary>
        /// <param name="reader">Reader value.</param>
        /// <param name="fieldName">Field name value.</param>
        /// <returns>The result value.</returns>
        public static int TryGetOrdinal(this IDataRecord reader, string fieldName)
        {
            var fieldCount = reader.FieldCount;

            for (var i = 0; i < fieldCount; i++)
            {
                var name = reader.GetName(i);
                if (name.Equals(fieldName, StringComparison.OrdinalIgnoreCase))
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