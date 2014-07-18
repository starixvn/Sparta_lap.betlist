// <copyright file="HSSFCellExtension.cs" company="Nexcel Solutions Vietnam" >
//  Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// <history>
//  <change who="jacob.phan" date="2012.5.8">Created</change>
// </history>

namespace Fanex.Athena.Excel.Extension
{
    using System;
    using Fanex.Common;
    using NPOI.SS.UserModel;

    /// <summary>
    /// HSSFCellExtensions class contains extension method for HSSFCell.
    /// </summary>
    internal static class HSSFCellExtension
    {
        /// <summary>
        /// Assign decimal value to cell.
        /// </summary>
        /// <param name="cell">Target cell.</param>
        /// <param name="value">The value to be set.</param>
        public static void SetCellValue(this ICell cell, decimal value)
        {
            cell.SetCellValue(Convert.ToDouble(value));
        }

        /// <summary>
        /// Assign object value to cell.
        /// </summary>
        /// <param name="cell">Target cell.</param>
        /// <param name="value">The value to be set.</param>
        public static void SetCellValue(this ICell cell, object value)
        {
            var valueType = value.GetType();

            if (valueType == typeof(bool))
            {
                cell.SetCellValue(Converter.To<bool>(value));
                return;
            }

            if (valueType == typeof(DateTime))
            {
                cell.SetCellValue(Converter.To<DateTime>(value));
                return;
            }

            if (valueType == typeof(decimal))
            {
                cell.SetCellValue(Converter.To<decimal>(value));
                return;
            }

            if (valueType == typeof(double))
            {
                cell.SetCellValue(Converter.To<double>(value));
                return;
            }

            if (valueType == typeof(int))
            {
                cell.SetCellValue(Converter.To<int>(value));
                return;
            }

            if (valueType == typeof(long))
            {
                cell.SetCellValue(Converter.To<long>(value));
                return;
            }

            if (valueType == typeof(string))
            {
                cell.SetCellValue(Converter.To<string>(value));
                return;
            }
        }
    }
}