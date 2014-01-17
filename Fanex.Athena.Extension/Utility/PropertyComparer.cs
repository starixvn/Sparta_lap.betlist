// -----------------------------------------------------------------------
// <copyright file="PropertyComparer.cs" company="Unknown">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Extension.Utility
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;

    /// <summary>
    /// Compare 2 property properties for sorting.
    /// </summary>
    /// <remarks>
    /// Taken from sample code in
    /// MSDN article.
    /// </remarks>
    /// <typeparam name="T">Type parameter.</typeparam>
    public class PropertyComparer<T> : IComparer<T>
    {                
        public PropertyComparer(string property, ListSortDirection direction)
        {            
            PropertyName = property;
            Direction = direction;
        }

        protected string PropertyName { get; set; }

        protected ListSortDirection Direction { get; set; }

        #region IComparer<T>

        /// <summary>
        /// Compare two values.
        /// </summary>
        /// <param name="x">First value.</param>
        /// <param name="y">Second value.</param>
        /// <returns>Compared result.</returns>
        public virtual int Compare(T x, T y)
        {
            // Get property values
            var firstValue = GetPropertyValue(x, PropertyName);
            var secondValue = GetPropertyValue(y, PropertyName);

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

        public bool Equals(T x, T y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }

        #endregion

        /// <summary>
        /// Compare two property values of any type.
        /// </summary>
        /// <param name="x">First value.</param>
        /// <param name="y">Second value.</param>
        /// <returns>Compared result.</returns>
        protected static int CompareAscending(object x, object y)
        {
            int result;

            if (x == null && y == null)
            {
                result = 0;
            }
            else if (x == null)
            {
                result = -1;
            }
            else if (y == null)
            {
                result = 0;
            }
            else if (x is IComparable)
            {
                result = ((IComparable)x).CompareTo(y);
            }
            else if (x.Equals(y))
            {
                result = 0;
            }
            else
            {
                result = x.ToString().CompareTo(y.ToString());
            }

            return result;
        }

        protected static int CompareDescending(object x, object y)
        {
            // Return result adjusted for ascending or descending sort order ie
            // multiplied by 1 for ascending or -1 for descending
            return CompareAscending(x, y) * -1;
        }

        protected static object GetPropertyValue(T value, string property)
        {
            // Get property
            PropertyInfo propertyInfo = value.GetType().GetProperty(property);

            // Return value
            return propertyInfo.GetValue(value, null);
        }
    }
}
