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
        public static bool HasColumn(this IDataRecord reader, string columnName)
        {
            var fieldCount = reader.FieldCount;

            for (var i = 0; i < fieldCount; i++)
            {
                var name = reader.GetName(i);
                if (name.Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
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