// -----------------------------------------------------------------------
// <copyright file="SystemConstants.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Core.DataDomain
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class SystemConstants
    {
        public const int PageSizeExcel = int.MaxValue - 1000;

        public static readonly int MinBaseCurrencyType = BaseCurrencyType.Gbp;

        private static readonly ReadOnlyCollection<string> _BaseCurrencyList = new ReadOnlyCollection<string>(
            new List<string> { "EUR", "USD", "GBP" });

        private static readonly ReadOnlyCollection<string> _newBaseCurrencyList = new ReadOnlyCollection<string>(
            new List<string> { "EUR", "USD", "GBP", "MYR", "CNY", "THB", "IDR" });

        public static ReadOnlyCollection<string> BaseCurrencyList
        {
            get
            {
                return _BaseCurrencyList;
            }
        }

        public static ReadOnlyCollection<string> NewBaseCurrencyList
        {
            get
            {
                return _newBaseCurrencyList;
            }
        }
    }
}