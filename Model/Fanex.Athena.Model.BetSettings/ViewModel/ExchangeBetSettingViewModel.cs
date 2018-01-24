// -----------------------------------------------------------------------
// <copyright file="BetSettingViewModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Fanex.Athena.Model.BetSettings.ViewModel
{
    public class ExchangeBetSettingViewModel : BetSettingBaseViewModel
    {
        public double Commission { get; set; }

        public bool IsNoInfo { get; set; }

        public IEnumerable<double> CommissionList { get; set; }
    }
}