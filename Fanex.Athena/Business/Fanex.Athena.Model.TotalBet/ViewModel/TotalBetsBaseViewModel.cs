// -----------------------------------------------------------------------
// <copyright file="TotalBetsBaseViewModel.cs" company="Nexcel">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.TotalBet.ViewModel
{
    using System.Collections.Generic;

    /// <summary>
    /// TotalBetsBaseViewModel class.
    /// </summary>
    public class TotalBetsBaseViewModel
    {
        public IList<string> LicenseeTypeList { get; set; }

        public IDictionary<string, int> CurrencyList { get; set; }
    }
}