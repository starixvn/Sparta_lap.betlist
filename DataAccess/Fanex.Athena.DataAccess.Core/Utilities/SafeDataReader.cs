namespace Fanex.Athena.DataAccess.Core.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.DataAccess.Core.Extensions;
    using Fanex.Common.Extensions;

    public class SafeDataReader
    {
        private readonly IDataReader _reader;

        private readonly HashSet<string> _existedColumns;

        public SafeDataReader(IDataReader reader, IEnumerable<string> columns)
        {
            _reader = reader;
            _existedColumns = new HashSet<string>();

            foreach (var item in columns)
            {
                if (reader.HasColumn(item))
                {
                    _existedColumns.Add(item);
                }
            }
        }

        public bool Read()
        {
            return _reader.Read();
        }

        public T GetValue<T>(string column, Func<object, T> func, T defaultValue)
        {
            if (_existedColumns.Contains(column))
            {
                var value = _reader[column];
                return func(value);
            }

            return defaultValue;
        }

        public object GetValue(string column, object defaultValue = null)
        {
            if (_existedColumns.Contains(column))
            {
                return _reader[column];
            }

            return defaultValue;
        }

        public string GetString(string column, string defaultValue = "")
        {
            if (_existedColumns.Contains(column))
            {
                return _reader.ToString(column);
            }

            return defaultValue;
        }

        public int GetInt(string column, int defaultValue = 0)
        {
            if (_existedColumns.Contains(column))
            {
                return _reader.ToInt(column);
            }

            return defaultValue;
        }

        public long GetLong(string column, long defaultValue = 0)
        {
            if (_existedColumns.Contains(column))
            {
                return _reader.ToLong(column);
            }

            return defaultValue;
        }

        public long? GetNullableLong(string column, long? defaultValue = null)
        {
            if (_existedColumns.Contains(column))
            {
                var value = _reader[column];
                if (value != DBNull.Value)
                {
                    return (long)value;
                }
            }

            return defaultValue;
        }

        public decimal GetDecimal(string column, decimal defaultValue = 0)
        {
            if (_existedColumns.Contains(column))
            {
                return _reader.ToDecimal(column);
            }

            return defaultValue;
        }

        public DateTime GetDateTime(string column, DateTime defaultValue)
        {
            if (_existedColumns.Contains(column))
            {
                return _reader.ToDateTime(column);
            }

            return defaultValue;
        }
    }
}