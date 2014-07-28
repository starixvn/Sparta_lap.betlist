﻿// -----------------------------------------------------------------------
// <copyright file="DictionaryPropertyComparer.cs" company="Unknown">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Extension.Utility
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Compare 2 property properties for sorting.
    /// </summary>
    /// <remarks>
    /// Taken from sample code in
    /// MSDN article.
    /// </remarks>
    /// <typeparam name="T">Type parameter.</typeparam>
    /// <typeparam name="TKey">Key type of dictionary.</typeparam>
    /// <typeparam name="TValue">Value type of dictionary.</typeparam>
    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Justification = "Reviewed.")]
    public class DictionaryPropertyComparer<T, TKey, TValue> : PropertyComparer<T>
    {
        private TKey _key;

        public DictionaryPropertyComparer(TKey key, string property, ListSortDirection direction)
            : base(property, direction)
        {
            _key = key;
        }

        #region IComparer<T>

        /// <summary>
        /// Compare two values.
        /// </summary>
        /// <param name="x">First value.</param>
        /// <param name="y">Second value.</param>
        /// <returns>Compared result.</returns>
        public override int Compare(T x, T y)
        {
            // Get property values
            var firstValue = (GetPropertyValue(x, PropertyName) as Dictionary<TKey, TValue>)[_key];
            var secondValue = (GetPropertyValue(y, PropertyName) as Dictionary<TKey, TValue>)[_key];

            // Determine sort order
            if (Direction == ListSortDirection.Ascending)
            {
                return CompareAscending(firstValue, secondValue);
            }
            else
            {
                return CompareDescending(firstValue, secondValue);
            }
        }
        #endregion
    }
}