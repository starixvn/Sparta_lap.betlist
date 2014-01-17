// -----------------------------------------------------------------------
// <copyright file="WinLossViewModel.cs" company="Nexcel">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Warehouse
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Warehouse;
    using Fanex.Athena.Models.MetaModel.Warehouse;

    /// <summary>
    /// WinLossViewModel class.
    /// </summary>
    public class WinLossViewModel : BaseViewModel
    {
        public WinLossMetaModel Param { get; set; }

        public IList<WinLoss> MainResult { get; set; }

        public IDictionary<string, decimal> TotalValues { get; set; }

        public IList<Licensee> LicenseeList { get; set; }
    }
}