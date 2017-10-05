namespace Fanex.Athena.DataAccess.Core.Utilities
{
    using System;

    public interface ISafeDataReader
    {
        DateTime GetDateTime(string column, DateTime defaultValue);

        decimal GetDecimal(string column, decimal defaultValue = 0);

        int GetInt(string column, int defaultValue = 0);

        long GetLong(string column, long defaultValue = 0);

        long? GetNullableLong(string column, long? defaultValue = default(long?));

        string GetString(string column, string defaultValue = "");

        object GetValue(string column, object defaultValue = null);

        T GetValue<T>(string column, Func<object, T> func, T defaultValue);

        bool Read();
    }
}