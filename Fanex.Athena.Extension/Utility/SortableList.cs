// -----------------------------------------------------------------------
// <copyright file="SortableList.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Extension.Utility
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>    
    /// TODO: Update summary.
    /// </summary>
    /// <typeparam name="T">Type parameter.</typeparam>
    public class SortableList<T> : List<T>
    {
        private string _propertyName;
        private bool _ascending;

        /// <summary>
        /// Sort list by property name.
        /// </summary>
        /// <param name="key">Key of item in dictionary.</param>
        /// <param name="propertyName">Property name to sort.</param>
        /// <param name="ascending">Sort ascending or descending.</param>
        /// <typeparam name="TKey">Key type of dictionary.</typeparam>
        /// <typeparam name="TValue">Value type of dictionary.</typeparam>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Reviewed.")]
        public void SortByDictionary<TKey, TValue>(TKey key, string propertyName, bool ascending)
        {
            // Flip the properties if the parameters are the same.
            if (_propertyName == propertyName && _ascending == ascending)
            {
                _ascending = !ascending;
            }
            else
            {
                // Else, new properties are set with the new values.
                _propertyName = propertyName;
                _ascending = ascending;
            }

            // Apply and set the sort, if items to sort
            var comparer = new DictionaryPropertyComparer<T, TKey, TValue>(key, _propertyName, ascending ? ListSortDirection.Ascending : ListSortDirection.Descending);
            this.Sort(comparer);
        }

        /// <summary>
        /// Sort list by property name.
        /// </summary>
        /// <param name="propertyName">Property name to sort.</param>
        /// <param name="ascending">Sort ascending or descending.</param>
        public void Sort(string propertyName, bool ascending)
        {
            // Flip the properties if the parameters are the same.
            if (_propertyName == propertyName && _ascending == ascending)
            {
                _ascending = !ascending;
            }            
            else
            {
                // Else, new properties are set with the new values.
                _propertyName = propertyName;
                _ascending = ascending;
            }
            
            // Apply and set the sort, if items to sort
            var comparer = new PropertyComparer<T>(propertyName, _ascending ? ListSortDirection.Ascending : ListSortDirection.Descending);
            this.Sort(comparer);
        }
    }
}
