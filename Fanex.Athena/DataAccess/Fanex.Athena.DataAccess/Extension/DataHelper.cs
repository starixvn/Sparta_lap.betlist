// -----------------------------------------------------------------------
// <copyright file="DataHelper.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Extension
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public sealed class DataHelper
    {
        private DataHelper()
        {
        }

        public static string GetCurrencyColumnName(string columnName, bool isConvert, string baseCurrency)
        {
            var columnNameSuffix = isConvert ? baseCurrency : string.Empty;

            columnName = columnName + columnNameSuffix;
            return columnName;
        }
    }
}