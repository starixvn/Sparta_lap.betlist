// -----------------------------------------------------------------------
// <copyright file="WinLossViewModel.cs" company="Nexcel">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Warehouse.ViewModel
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Warehouse.DTO;
    using Fanex.Athena.Model.Warehouse.MetaModel;
    using Model.Core.ViewModel;

    /// <summary>
    /// WinLossViewModel class.
    /// </summary>
    public class WinLossViewModel : BaseViewModel
    {
        public WinLossMetaModel Param { get; set; }

        public IList<WinLoss> MainResult { get; set; }

        public IDictionary<string, decimal> TotalValues { get; set; }
    }
}