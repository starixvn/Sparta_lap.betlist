// -----------------------------------------------------------------------
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
        private readonly TKey _key;

        public DictionaryPropertyComparer(TKey key, string property, ListSortDirection direction)
            : base(property, direction)
        {
            _key = key;
        }

        #region IComparer<T>

        public override int Compare(T x, T y)
        {
            var firstValue = (GetPropertyValue(x, PropertyName) as Dictionary<TKey, TValue>)[_key];
            var secondValue = (GetPropertyValue(y, PropertyName) as Dictionary<TKey, TValue>)[_key];

            return Direction == ListSortDirection.Ascending ?
                CompareAscending(firstValue, secondValue) :
                CompareDescending(firstValue, secondValue);
        }

        #endregion IComparer<T>
    }
}