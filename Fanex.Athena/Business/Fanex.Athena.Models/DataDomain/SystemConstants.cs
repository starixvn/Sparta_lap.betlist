// -----------------------------------------------------------------------
// <copyright file="SystemConstants.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DataDomain
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class SystemConstants
    {
        private static ReadOnlyCollection<string> _baseCurrencyList = new ReadOnlyCollection<string>(
            new List<string> { "EUR", "USD", "GBP" });

        public static ReadOnlyCollection<string> BaseCurrencyList
        {
            get
            {
                return _baseCurrencyList;
            }
        }
    }
}