// -----------------------------------------------------------------------
// <copyright file="DictionaryPropertyComparer.cs" company="Unknown">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Extension.Utility
{
    using System.Collections;
    using System.ComponentModel;

    public class DictionaryPropertyComparer<T> : PropertyComparer<T>
    {
        private readonly object _key;

        public DictionaryPropertyComparer(object key, string property, ListSortDirection direction)
            : base(property, direction)
        {
            _key = key;
        }

        #region IComparer<T>

        public override int Compare(T x, T y)
        {
            var firstValue = (GetPropertyValue(x, PropertyName) as IDictionary)[_key];
            var secondValue = (GetPropertyValue(y, PropertyName) as IDictionary)[_key];

            return Direction == ListSortDirection.Ascending ?
                CompareAscending(firstValue, secondValue) :
                CompareDescending(firstValue, secondValue);
        }

        #endregion IComparer<T>
    }
}