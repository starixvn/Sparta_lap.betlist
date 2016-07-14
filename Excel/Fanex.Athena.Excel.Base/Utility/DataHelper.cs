// --------------------------------------------------------------------------
// <copyright file="DataHelper.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Dave.Bui" date="2012.4.11">Created</change>
// </history>
// ----------------------------------------------------------------------

namespace Fanex.Athena.Excel.Base.Utility
{
    using System.Data;
    using Common;
    using Entities;

    /// <summary>
    /// Data helper - Utility method to for getting currency value in data table.
    /// </summary>
    public static class DataHelper
    {
        public static string GetCurrencyColumnName(string columnName, ConvertingOption option)
        {
            string columnNameSuffix = option.IsConvert
                                      ? option.BaseCurrency
                                      : string.Empty;

            columnName = columnName + columnNameSuffix;
            return columnName;
        }

        /// <summary>
        ///     Edit by Alex.
        ///     @rev:check existing column before taking value
        ///     @Create_time:20120709.
        /// </summary>
        /// <param name="row">The data row.</param>
        /// <param name="columnName">The data table column name.</param>
        /// <param name="option">Converting Option.</param>
        /// <returns>Currency value.</returns>
        public static decimal GetCurrencyValue(DataRow row, string columnName, ConvertingOption option)
        {
            string currencyColumnName = GetCurrencyColumnName(columnName, option);
            return row.Table.Columns.Contains(currencyColumnName) ? Converter.To<decimal>(row[currencyColumnName]) : 0;
        }
    }
}