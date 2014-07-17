// -----------------------------------------------------------------------
// <copyright file="DictionaryPropertyComparer.cs" company="Unknown">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Extension.Utility
{
    using System.Collections;
    using System.ComponentModel;

    /// <summary>
    /// Compare 2 property properties for sorting.
    /// </summary>
    /// <remarks>
    /// Taken from sample code in
    /// MSDN article.
    /// </remarks>
    /// <typeparam name="T">Type parameter.</typeparam>
    public class DictionaryPropertyComparer<T> : PropertyComparer<T>
    {
        private object _key;

        public DictionaryPropertyComparer(object key, string property, ListSortDirection direction)
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
            var firstValue = (GetPropertyValue(x, PropertyName) as IDictionary)[_key];
            var secondValue = (GetPropertyValue(y, PropertyName) as IDictionary)[_key];

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