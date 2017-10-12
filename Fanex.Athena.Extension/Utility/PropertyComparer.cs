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

        public virtual int Compare(T x, T y)
        {
            var firstValue = GetPropertyValue(x, PropertyName);
            var secondValue = GetPropertyValue(y, PropertyName);

            return Direction == ListSortDirection.Ascending ?
                CompareAscending(firstValue, secondValue) :
                CompareDescending(firstValue, secondValue);
        }

        #endregion IComparer<T>

        protected static int CompareAscending(object x, object y)
        {
            int result;

            if (y == null)
            {
                result = 0;
            }
            else if (x == null)
            {
                result = -1;
            }
            else
            {
                var value = x as IComparable;

                result = value != null
                    ? value.CompareTo(y)
                    : (x.Equals(y) ? 0 : string.Compare(x.ToString(), y.ToString(), StringComparison.OrdinalIgnoreCase));
            }

            return result;
        }

        protected static int CompareDescending(object x, object y)
        {
            // Return result adjusted for ascending or descending sort order ie
            // multiplied by 1 for ascending or -1 for descending
            return CompareAscending(x, y) * -1;
        }

        protected static object GetPropertyValue(T value, string propertyName)
        {
            var property = value.GetType().GetProperty(propertyName);

            return property.GetValue(value, null);
        }
    }
}