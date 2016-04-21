// -----------------------------------------------------------------------
// <copyright file="CustomerBalanceViewModel.cs" company="Nexcel">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Model.Core.ViewModel;

    /// <summary>
    /// CustomerBalanceViewModel class.
    /// </summary>
    public class CustomerBalanceViewModel : BaseViewModel
    {
        public bool IsValidTimeRequest { get; set; }

        public IList<CustBalance> DataList { get; set; }

        public CustBalance TotalData { get; set; }

        public CustomerBalanceMetaModel Param { get; set; }
    }
}